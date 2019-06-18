## Beach Day
A Xamarin Forms Mobile app that is used to provide the user with some tools for their day at the beach.  Currently includes a tanning alert tool, simple checklist, and a weather forecast page.  Currently only tested on Android (Having difficulty finding a Apple product).  Features simple Google Analytics, simple SQLite databases for persistent data and Weather Data pulled from a RESTful API.


### Prerequisites

What things you need to install the software and how to install them.

```
Installation of Visual Studio with the "Mobile Development with .NET" Workload.  
For Android builds, get a Google API key for Android SDK and SHA-1 fingerprint for that key.
https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/map
https://developers.google.com/maps/documentation/android-sdk/get-api-key
```

### Issues
* Need to Implement Run-Time Permission Requests (for Fine/Course location) SDK build for Android is set to less than 22.
* Making the features of the app more "discoverable" (how to inform the user what the tanning tool is used for).
* Tanning Tool timer needs to be reworked (Task.Delay() value is arbitrary it leads to displayed seconds being either sped up or slowed down, perhaps use Timer.Interval function).
* Forcing the user from creating beach places that are not actual beach locations
* The checklist's strikethrough graphic should be Red
*

### Considerations For Improvements
* Overhaul spacing of data for better UI
* Rework the Horizontal Listview
* Add in the ability for the user to type up their own notes about their beaches.
* Implement a real Splash Screen upon app start-up
* Implement Beach Facts page with information regarding user's beach
* Checklist could use delete icons instead of a long-press Context Menu.
* Implement MVVM
* add an Icon for this App (replace default Xamarin Forms icon).
* Geocode user's Beach Places.


### Author(s)

* **Shane Laskowski**

### 3rd Party Software

* SQLite-net-pcl by Frank A. Krueger
* Xam.Plugin.SimpleAudioPlayer by Adrian Stevens
* Newtonsoft.Json by James Newton-King
* Xamarin.Forms.GoogleMaps by amay007
* Ansuria.XFGloss by Tommy Baggett