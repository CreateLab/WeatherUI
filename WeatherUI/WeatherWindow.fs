namespace WeatherUI

open System
open System.Threading.Channels

module WeatherWindow =

    open Avalonia.Controls
    open Avalonia.FuncUI.DSL

    type State =
        { text: string }



    let t (t: string) = t

    let cw (text: string): string =
        Console.WriteLine(text)
        text

    let init = { text = cw "Hello World" }
    

    type Msg = Text of string
     let getAsync (url:string) = 
        async {
            let httpClient = new System.Net.Http.HttpClient()
            let! response = httpClient.GetAsync(url) |> Async.AwaitTask
            response.EnsureSuccessStatusCode () |> ignore
            let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
            return content 
            }

         


    let update (msg: Msg) (state: State): State =
        match msg with
        |  Text text -> { state with text = text }
    

    let view (state: State) (dispatch) =
        StackPanel.create [ StackPanel.children [ TextBlock.create [ TextBlock.text (state.text) ] ] ]
