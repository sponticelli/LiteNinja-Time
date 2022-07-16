# LiteNinja-Time
A library of Time-related classes for Unity3D

## Features

### Duration
A static class that can parse a string to TimeSpan and vice versa.

#### Parse
Parse a string into a TimeSpan. 
```
var timespan = Duration.Parse("90m");
timespan = Duration.Parse("1h30m");
timespan = Duration.Parse("1.5h");
```
If the string is invalid, an exception will be thrown.

#### ParseSafe
`ParseSafe` try to parse even if the string is not in the correct format.
```
var timespan = Duration.ParseSafe("90m1parsec");
Debug.Log(timespan.ToDuration(); // "1h30m"
```

#### Parseable
You can check if the string is in the correct format by using the `Parseable` method.

#### ToDuration
Convert a Duration to a string. The string can be in the format `{weeks}w{days}d{hours}h{minutes}m{seconds}s{milliseconds}ms`, with only the units with a value being included.
```
var timespan = Duration.Parse("90m");
Debug.Log(timespan.ToDuration()); // "1h30m"
```

#### Tests
A good coverage of the Duration class is provided by the tests. 
Peek at them to see how you can use the class.

## Install through [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

Unity's own Package Manager supports [importing packages through a URL to a Git repo](https://docs.unity3d.com/Manual/upm-ui-giturl.html):

1. First, on this repository page, click the "Clone or download" button, and copy over this repository's HTTPS URL.
2. Then click on the + button on the upper-left-hand corner of the Package Manager, select "Add package from git URL..." on the context menu, then paste this repo's URL!
