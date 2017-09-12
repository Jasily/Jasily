# Jasily.Features

This module was design for attach some to the type.

## How to use

For example:

``` cs
// build provider
var builder = new FeaturesBuilder();
builder.RegisterFeature<string, StringBuilder>(z => new StringBuilder(z.Source));
var provider = this.CreateFeaturesProvider();

// get factory for `string`
var factory = provider.GetFeaturesFactory<string>();

// get `StringBuilder`
var sb = factory.TryCreateFeature<StringBuilder>("50", false);
Console.WriteLine(sb.ToString()); // 50
```

### Work with IoC

Require reference from NuGet:

* `Microsoft.Extensions.DependencyInjection`
* `Microsoft.Extensions.Options`

``` cs
using Microsoft.Extensions.DependencyInjection;

// build service provider
var sc = new ServiceCollection();
var builder = sc.AddFeatures();
builder.RegisterFeature<string, StringBuilder>(z => new StringBuilder(z.Source));
var provider = sc.BuildServiceProvider();

// get factory from service provider
var factory = provider.GetRequiredService<IFeaturesFactory<string>>();
```
