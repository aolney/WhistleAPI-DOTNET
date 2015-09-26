# WhistleAPI-DOTNET
Unofficial API for Whistle Dog Activity Monitor in C#

Provides a rough API for the Whistle Activity Monitor www.whistle.com

In order to make use of this API you must have a Whistle device and have created an account to register it.

This API mirrors the calls the Whistle app makes to retrieve data. The results are returned in JSON.

Here we make use of the Newtonsoft JSON serialization NuGet package and weakly type the returned data.

This is intentional to make it more robust to API changes since the API is not public.
