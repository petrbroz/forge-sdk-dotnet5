# forge-sdk-dotnet5

Experimental [Autodesk Forge](https://forge.autodesk.com) SDK for [.NET 5.0 (Release Candidate 1)](https://devblogs.microsoft.com/dotnet/announcing-net-5-0-rc-1), with a couple of sample applications.

## Development

### Prerequisites

- [Autodesk Forge application](https://forge.autodesk.com/en/docs/oauth/v2/tutorials/create-app)
- [Git](https://git-scm.com)
- [.NET 5.0 (Release Candidate 1) SDK](https://dotnet.microsoft.com/download/dotnet/5.0)

### Option 1: Using Visual Studio Code

- Clone the repository
- Open the solution folder with [Visual Studio Code](https://code.visualstudio.com)
- Create a new _.env_ file in the solution folder, specifying `FORGE_CLIENT_ID` and `FORGE_CLIENT_SECRET` environment variables:
    ```
    FORGE_CLIENT_ID=foo
    FORGE_CLIENT_SECRET=bar
    ```
- Launch one of the predefined configurations ("Launch Sample Console App" or "Launch Sample Server App")

### Option 2: Manual Setup

- Clone the repository
- Build the solution
    ```bash
    cd forge-sdk-dotnet5
    dotnet build
    ```
- Set the environment variables `FORGE_CLIENT_ID` and `FORGE_CLIENT_SECRET`
    ```bash
    # on Windows
    set FORGE_CLIENT_ID=foo
    set FORGE_CLIENT_SECRET=bar
    # on *nix platforms
    export FORGE_CLIENT_ID=foo
    export FORGE_CLIENT_SECRET=bar
    ```
- Run the sample console application
    ```bash
    cd sample-console-app
    dotnet run
    ```