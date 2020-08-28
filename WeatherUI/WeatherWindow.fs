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
 

    let update (msg: Msg) (state: State): State =
        match msg with
        |  Text text -> { state with text = text }
    

    let view (state: State) (dispatch) =
        StackPanel.create [ StackPanel.children [ TextBlock.create [ TextBlock.text (state.text) ] ] ]
