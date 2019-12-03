# WhistleAPI-DOTNET

## Version 2 : 12/3/2019

This is v2 of [WhistleAPI-DOTNET](https://github.com/aolney/WhistleAPI-DOTNET).
The original version was in C# and used WebClient.
For some reason, it just stopped working.

The same calls work in F# using [Fsharp.Data](https://fsharp.github.io/FSharp.Data/), so I decided to scrap the C# version and create this notebook with the minimal F# code needed to access the API.

The original code was informed by [an old description of the unofficial API](http://jared.wuntu.org/whistle-dog-activity-monitor-undocumented-api/).
In this new version, I loaded up [Charles Proxy](https://www.charlesproxy.com/) to [intercept encrypted traffic](https://medium.com/@hackupstate/using-charles-proxy-to-debug-android-ssl-traffic-e61fc38760f7) between the [Whistle Legacy app](https://play.google.com/store/apps/details?id=com.whistle.WhistleApp&hl=en_US) and the API endpoints.
I didn't notice any differences that were breaking, though it seems the app is sending additional information in the headers that's not needed to make the API work.

The entire API has not been implemented; that would be extremely tedious.
However, unlike v1, I'm providing a semi-complete domain model, using [FSharp.Json](https://vsapronov.github.io/FSharp.Json/) for serialization.
The API below represents what I consider to be the core interesting API.
If you want more, then see the comment in `AuthenticatedGet` to return raw JSON.

## Version 1 : 9/26/2015

Unofficial API for Whistle Dog Activity Monitor in C#

Provides a rough API for the Whistle Activity Monitor www.whistle.com

In order to make use of this API you must have a Whistle device and have created an account to register it.

This API mirrors the calls the Whistle app makes to retrieve data. The results are returned in JSON.

Here we make use of the Newtonsoft JSON serialization NuGet package and weakly type the returned data.

This is intentional to make it more robust to API changes since the API is not public.
