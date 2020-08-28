namespace WeatherUI

open System
open System.Threading.Channels
open Elmish

module WeatherWindow =

    open Avalonia.Controls
    open Avalonia.FuncUI.DSL

    type State =
        { text: string }



    let t (t: string) = t

    let cw (text: string): string =
        Console.WriteLine(text)
        text

    
    

    type Msg = Text of string
 
    

   
        
    let init  =
        {text = ""}
    let update (msg: Msg) (state: State): State =
        match msg with
        |  Text text -> { state with text = text }
    

    let view (state: State) (dispatch) =
        StackPanel.create [ StackPanel.children [ TextBlock.create [ TextBlock.text (state.text) ] ] ]
