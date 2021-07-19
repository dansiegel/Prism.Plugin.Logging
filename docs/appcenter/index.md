# App Center

App Center is a very popular and FREE platform provided by Microsoft for tracking Analytics about your users and collecting crash analytics. With support for most Xamarin applications this is a very good platform to use when developing Prism.Forms applications.

For more information see the [App Center](https://appcenter.ms/) website.

## Setup

The App Center Logger requires no explicit setup in terms of registering additional configuration classes. You will however need to be sure to start the App Center Analytics and Crashes using their standard API's.

```c#
AppCenter.Start("{your app secret}", typeof(Analytics), typeof(Crashes));

// For Xamarin.Forms
AppCenter.Start("ios:{your ios app secret};uwp{your uwp app secret};android:{your android app secret};", typeof(Analytics), typeof(Crashes));

```
