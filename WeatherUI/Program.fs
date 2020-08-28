namespace WeatherUI


open System
open System.Threading.Tasks
open Avalonia
open Avalonia.Threading
open Elmish
open Avalonia.FuncUI.Components.Hosts
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish
open Avalonia.Controls.ApplicationLifetimes

type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "WeatherUI"
        base.Width <- 400.0
        base.Height <- 400.0
        
        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
        let getAsync (url:string) = 
            async {
                Task.Delay 20000 |> Async.AwaitTask
                let httpClient = new System.Net.Http.HttpClient()
                let! response = httpClient.GetAsync(url) |> Async.AwaitTask
                response.EnsureSuccessStatusCode () |> ignore
                let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
                return content 
            }
            
        let timer (_state: WeatherWindow.State) =
            let sub (dispatch: WeatherWindow.Msg -> unit) =        
                getAsync "https://www.google.com"|> Async.RunSynchronously  |> WeatherWindow.Msg.Text |> dispatch      
            Cmd.ofSub sub

         

        Elmish.Program.mkSimple (fun () -> WeatherWindow.init) WeatherWindow.update WeatherWindow.view
        |> Program.withHost this
        |> Program.withSubscription timer
        |> Program.withConsoleTrace
        |> Program.run
        
type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Load "avares://Avalonia.Themes.Default/DefaultTheme.xaml"
        this.Styles.Load "avares://Avalonia.Themes.Default/Accents/BaseDark.xaml"

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)