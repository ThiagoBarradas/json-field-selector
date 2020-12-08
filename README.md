[![Build Status](https://barradas.visualstudio.com/Contributions/_apis/build/status/NugetPackage/JsonFieldSelector?branchName=develop)](https://barradas.visualstudio.com/Contributions/_build/latest?definitionId=23&branchName=develop)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ThiagoBarradas_json-field-selector&metric=alert_status)](https://sonarcloud.io/dashboard?id=ThiagoBarradas_json-field-selector)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ThiagoBarradas_json-field-selector&metric=coverage)](https://sonarcloud.io/dashboard?id=ThiagoBarradas_json-field-selector)
[![NuGet Downloads](https://img.shields.io/nuget/dt/JsonFieldSelector.svg)](https://www.nuget.org/packages/JsonFieldSelector/)
[![NuGet Version](https://img.shields.io/nuget/v/JsonFieldSelector.svg)](https://www.nuget.org/packages/JsonFieldSelector/)

# Json Field Selector 

Select fields from a object, jtoken or a string (json), don't care if property is in depth objects or into an array. 

Very useful to allows users to get a response with few fields by your choice, reducing you API response payload size, for example.

# Sample

```c#

var example = new 
{
	SomeValue = "Some value 1",
	SomeValue2 = "Some value 2",
	SomeValue3 = new 
	{
		SomeValue4 = "Some value 4",
		SomeValue5 = new 
		{
			SomeValue6 = "123123"
		}
	},
	SomeValue6 = new object[] 
	{
		new 
		{
			SomeValue7 = "Some value 7",
			SomeValue8 = "Some value 8"
		},
		new 
		{
			SomeValue7 = "Some value 7",
			SomeValue8 = "Some value 8"
		}
	}
};

var fields = new string[] { "SomeValue", "SomeValue3", "SomeValue6.SomeValue7" }; 
var filteredObj = example.SelectFieldsFromObject<object>(fields); 

// OR
// var fields = "SomeValue,SomeValue3,SomeValue6.SomeValue7"; 
// var separator = ',';
// var filteredObj = example.SelectFieldsFromObject<object>(fields, separator); 

var json = JsonConvert.SerializeObject(filteredObj);

Console.WriteLine(json);

```

Output
```json
{
	"SomeValue": "Some value 1",
	"SomeValue3": 
	{
		"SomeValue4": "Some value 4",
		"SomeValue5": 
		{
			"SomeValue6": "123123"
		}
	},
	"SomeValue6" : [
		{
			"SomeValue7": "Some value 7"
		},
		{
			"SomeValue7": "Some value 7"
		}
	]
};
```

## Install via NuGet

```
PM> Install-Package JsonFieldSelector
```

## How can I contribute?
Please, refer to [CONTRIBUTING](.github/CONTRIBUTING.md)

## Found something strange or need a new feature?
Open a new Issue following our issue template [ISSUE_TEMPLATE](.github/ISSUE_TEMPLATE.md)

## Changelog
See in [nuget version history](https://www.nuget.org/packages/JsonFieldSelector)

## Did you like it? Please, make a donate :)

if you liked this project, please make a contribution and help to keep this and other initiatives, send me some Satochis.

BTC Wallet: `1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX`

![1G535x1rYdMo9CNdTGK3eG6XJddBHdaqfX](https://i.imgur.com/mN7ueoE.png)
