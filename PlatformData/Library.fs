namespace FSLibrary

open System
open System.IO
open System.Threading.Tasks

[<Measure>] type pixel

/// Core interface for signals
type ISignal<'a> =
    /// The current value of the type
    abstract member Value : 'a with get
    abstract member ValueChanged : IEvent<'a>

[<RequireQualifiedAccess>]
module Signal =
    let constant(x:'a) =
        {   new ISignal<'a> with
                member _.Value = x
                member _.ValueChanged = Event<'a>().Publish
        }

type OS =
    | UWP = 0
    | IOS = 1
    | Droid = 2
    | Mac = 3
    | WPFPreview = 4

type DeviceType =
    | Phone = 0
    | Tablet = 1
    | Desktop = 2

[<Struct>]
type SuggestedFilenameWithoutExtension private (sfn: string) =
    member _.SuggestedFilenameWithoutExtensions = sfn
    static member Create(s: string) =
        let excludedChars = Path.GetInvalidFileNameChars()
        SuggestedFilenameWithoutExtension(s |> String.filter(fun c -> excludedChars |> Array.forall((<>) c)))

[<RequireQualifiedAccess>]
type FileType =
    | CSV
    | Text
    | Svg
    | Pdf
    /// Returns an extension, without the leading "."
    member t.Extension =
        match t with
        | CSV -> "csv"
        | Text -> "txt"
        | Svg -> "svg"
        | Pdf -> "pdf"

type OpenOrSave = | Open = 0 | Save = 1

[<RequireQualifiedAccess>]
type FileContents =
    | String of string
    | Bytes of byte[]
    static member Lines(lines: string seq) =
        String.Join("\r\n", lines) |> String

type FileSaveData(suggestedFileNameWithoutExtension:SuggestedFilenameWithoutExtension, contents: FileContents, fileType:FileType) =
    member _.SuggestedFileNameWithoutExtension = suggestedFileNameWithoutExtension.SuggestedFilenameWithoutExtensions
    /// The extension, not containing a dot
    member _.Extension = fileType.Extension
    member _.Content = contents
    member _.FileType = fileType

/// File load data with permitted file type. If none are given, then allow any files
type FileLoadData(restrictedFileType:FileType option) =
    member _.RestrictedFileType = restrictedFileType

[<RequireQualifiedAccess; Struct>]
type FileLoadError =
    | NoFile
    /// File cannot be read as text
    | InvalidFileFormat
    member t.Message =
        match t with
        | NoFile -> "No file loaded."
        | InvalidFileFormat -> "The file format is invalid."

type FileLoaderSaver private (saveFile:Func<FileSaveData, Task<bool>> voption, openFile:Func<FileSaveData, Task<bool>>, loadFileAsString:Func<FileLoadData, Task<Result<string, FileLoadError>>> voption) =
    /// Save file dialog - MUST be called from the UI Thread
    member _.SaveFileAsync =
        saveFile |> ValueOption.map(fun sFile ->
            fun (fsd:FileSaveData) -> sFile.Invoke(fsd))
    /// Open file - MUST be called from the UI Thread
    member _.OpenFileAsync =
        fun (fsd:FileSaveData) -> openFile.Invoke(fsd)
    /// Load file dialog - MUST be called from the UI Thread
    member _.LoadFileAsStringAsync =
        loadFileAsString |> ValueOption.map(fun lFile ->
            fun (fld:FileLoadData) -> lFile.Invoke(fld))
    static member OpenOnly(openFile: Func<FileSaveData, Task<bool>>) =
        FileLoaderSaver(ValueNone, openFile, ValueNone)

type Sharable(text:string, uri:string) =
    member _.Text = text
    member _.Uri = uri

type IPlatformData =
    // TODO remove OS from IPlatformData, since it's duplicated when being passed in to Diagnostics.fs
    // Either put IPlatformData in VMs, or split in two (skia vs non-skia)
    abstract member OS: OS
    abstract member DeviceType: DeviceType
    /// The number of pixels for each XF unit on the device
    abstract member DpiScaling: ISignal<float32<pixel>>
    abstract member GetSkiaTypefacesFromAssetFont: unit -> Task<string>
    abstract member OpenAppInStore: Func<Task>
    abstract member FileLoaderSaver: FileLoaderSaver

    /// Use the device to share a text string with an associated uri
    /// It appears the subject is used just on Android in the case of e.g. an email share
    abstract member Share:Sharable -> Task

    abstract member Open:Uri -> Task

    abstract member OpenFile:Func<FileSaveData, Task<bool>>

    abstract member CallOnRenderThread: Action -> Task
    abstract member CallOnRenderThread<'a> : Func<'a> -> Task<'a>
    abstract member CallTaskOnRenderThread: Func<Task> -> Task
    abstract member CallTaskOnRenderThread<'a> : Func<Task<'a>> -> Task<'a>

[<RequireQualifiedAccess>]
module Log =
    let MessageToConsole(msg: string) =
        Console.WriteLine("ѪCWL: " + msg)

type PassedIn(pd: IPlatformData) =
    do  Log.MessageToConsole("About to print OS")
        Log.MessageToConsole(pd.OS.ToString())
        Log.MessageToConsole("About to print DeviceType")
        Log.MessageToConsole(pd.DeviceType.ToString())