# LiteNinja-Time
A library of Time-related classes for Unity3D

## Features

### Duration
A class for representing a duration of time.

#### Parse
Parse a string into a Duration. The string can be in the format `{weeks}w{days}d{hours}h{minutes}m{seconds}s{milliseconds}ms`.
```
var duration = Duration.Parse("90m");
duration = Duration.Parse("1h30m");
duration = Duration.Parse("1.5h");
```
Parse try to parse even if the string is not in the correct format.
You can check if the string is in the correct format by using the `Parseable` method.

#### ToString
Convert a Duration to a string. The string can be in the format `{weeks}w{days}d{hours}h{minutes}m{seconds}s{milliseconds}ms`, with only the units with a value being included.
```
var duration = Duration.Parse("90m");
Debug.Log(duration.ToString()); // "1h30m"
```

#### Operators
The following operators are supported: +, -, *, /, ==, !=, <, >, <=, >=.
```
var duration = new Duration("6d");
var duration2 = new Duration("1d");
duration += duration2; // duration = 1w

duration = new Duration("1d");
duration /= 2; // duration = 12h
```

#### DateTime
Some extension methods for manipulating a DateTime with a duration.
```
var duration = new Duration("1d");
var dateTime = new DateTime(2020, 1, 1);
dateTime += duration; // dateTime = 2020-01-02

dateTime = new DateTime(2020, 1, 1);
dateTime -= duration; // dateTime = 2019-12-31

dateTime = new DateTime(2020, 1, 1);    
dateTime.After(duration); // 2020-01-02

dateTime = new DateTime(2020, 1, 1);
dateTime.Before(duration); // 2019-12-31
```

Other extensions methods allow to calculate the duration between two DateTime.
```
var dateTime1 = new DateTime(2020, 1, 1);
var dateTime2 = new DateTime(2020, 1, 2);
var duration = dateTime2.Between(dateTime1); // duration = 1d
```

It is obviously convert a duration to a TimeSpan and the other way around.
```
var duration = new Duration("1d");
var timeSpan = duration.ToTimeSpan(); 

var timeSpan = new TimeSpan(1, 0, 0, 0);
var duration = timeSpan.ToDuration(); // duration = 1d
```


#### Tests
A good coverage of the Duration class is provided by the tests. 
Peek at them to see how you can use the class.

## Install through [Unity Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html)

Unity's own Package Manager supports [importing packages through a URL to a Git repo](https://docs.unity3d.com/Manual/upm-ui-giturl.html):

1. First, on this repository page, click the "Clone or download" button, and copy over this repository's HTTPS URL.
2. Then click on the + button on the upper-left-hand corner of the Package Manager, select "Add package from git URL..." on the context menu, then paste this repo's URL!
