#r "paket: groupref build //"
#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.Tools.Git

let initializeContext () =
    let execContext = Context.FakeExecutionContext.Create false "build.fsx" []
    Context.setExecutionContext (Context.RuntimeContext.Fake execContext)

initializeContext ()

let buildConfig =
    Environment.environVarOrDefault "buildMode" "Release"
    |> DotNet.BuildConfiguration.fromString

let buildArtifactPath = Path.getFullName "./artifacts"
let baseVersion = "2.0.1"

let envVersion  = Environment.environVarOrDefault "APPVEYOR_BUILD_VERSION" (baseVersion + ".0")
let buildVersion = envVersion.Substring(0, envVersion.LastIndexOf('.'))
let version      = (SemVer.parse buildVersion).ToString()
let fileVersion  = Environment.environVarOrDefault "APPVEYOR_BUILD_VERSION" (version + ".0")

let branchName =
    Environment.environVarOrDefault "APPVEYOR_REPO_BRANCH" (Information.getBranchName ".")

let nugetVersion =
    if branchName = "master" then version
    else fileVersion + "-" + branchName

let infoVersion =
    let label =
        if branchName = "master" then ""
        else sprintf " (%s/%s)" branchName (Information.getCurrentSHA1 ".").[0..7]
    fileVersion + label

let versionProps =
    [ "Version",              nugetVersion
      "AssemblyVersion",      fileVersion
      "FileVersion",          fileVersion
      "InformationalVersion", infoVersion ]

Trace.tracefn "Version: %s" version

Target.create "Clean" (fun _ ->
    Directory.ensure buildArtifactPath
    Shell.cleanDir buildArtifactPath
)

Target.create "Build" (fun _ ->
    DotNet.build (fun p ->
        { p with
            Configuration = buildConfig
            MSBuildParams = { p.MSBuildParams with Properties = versionProps } }
    ) "./src/EmailReplyParser.sln"
)

Target.create "Test" (fun _ ->
    !! "src/*.Tests/*.csproj"
    |> Seq.iter (DotNet.test (fun p -> { p with Configuration = buildConfig }))
)

Target.create "Package" (fun _ ->
    !! "src/*/*.csproj" -- "src/*.Tests/*.csproj"
    |> Seq.iter (DotNet.pack (fun p ->
        { p with
            Configuration = buildConfig
            OutputPath    = Some buildArtifactPath
            MSBuildParams = { p.MSBuildParams with Properties = versionProps @ [ "IncludeSymbols", "true"; "IncludeSource", "true" ] } }))
)

"Clean" ==> "Build" ==> "Test" ==> "Package"

Target.runOrDefault "Package"
