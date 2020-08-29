namespace WeatherUI

open System
open System.Threading.Channels
open System.Threading.Tasks
open Elmish
open Elmish
open System.Text.Json
open System.Collections.Generic
open System.Text

module WeatherWindow =

    open Avalonia.Controls
    open Avalonia.FuncUI.DSL

    /// this is because I'm using System.Text.Json, I belive Newtonsoft.Json doesn't need it
    [<CLIMutable>]
    type Todo =
        { userId: int
          id: int
          title: string
          completed: bool }

    type State =
        { text: string; todos: list<Todo> }

    let t (t: string) = t

    let cw (text: string): string =
        Console.WriteLine(text)
        text

    type Msg = 
        | Text of  string
        | SetTodos of list<Todo>
    
    let getAsync (url:string) = 
        async {
                let httpClient = new System.Net.Http.HttpClient()
                let! response = httpClient.GetAsync(url) |> Async.AwaitTask
                response.EnsureSuccessStatusCode () |> ignore
                let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
                let span = 
                    let bytes = Encoding.UTF8.GetBytes(content)
                    ReadOnlySpan(bytes)
                let todos = JsonSerializer.Deserialize<IEnumerable<Todo>>(span)
                // ensure to return a Msg from your async action
                return SetTodos (todos |> Seq.toList)
            }
        |>  Cmd.OfAsync.result 
  
    /// Ensure your init fn returns a State * Cmd<Msg> Tuple      
    let init : State * Cmd<Msg> =
        { text = ""; todos = [] } , getAsync "https://jsonplaceholder.typicode.com/todos?_page=1&_limit=5"

    /// when you are using Program.mkProgram you need to return a State * Cmd<Msg> Tuple      
    /// from in your update function as well
    let update (msg: Msg) (state: State): State * Cmd<Msg> =
        match msg with
        | Text text -> { state with text = text }, Cmd.none
        | SetTodos todos -> { state with todos = todos }, Cmd.none
    

    let view (state: State) (dispatch) =
        StackPanel.create [ 
            StackPanel.children [ 
                TextBlock.create [ 
                    TextBlock.text "Todos: "
                ] 
                for todo in state.todos do 
                    StackPanel.create [
                        StackPanel.spacing 12.
                        StackPanel.children [
                            TextBlock.create [ 
                                TextBlock.text (todo.title)
                            ] 
                            TextBlock.create [ 
                                TextBlock.text (todo.completed |> string)
                            ] 
                        ]
                    ]
            ] 
        ]
