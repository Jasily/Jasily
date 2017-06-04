# Complex Service

The library is base on `Microsoft.Extensions.DependencyInjection`.

## How To Use

``` cs
var sc = new ServiceCollection();
sc.AddComplexService().AddTransient(typeof(IInterface1), typeof(Class1));
var provider = sc.BuildServiceProvider();
```

Then your can get service use:

``` cs
var factory = provider.GetService<IComplexServiceFactory>();
var type = factory.GetClosedServiceTypeOrNull(typeof(IInterface1));
var service = provider.GetService(type);
```

or just:

``` cs
var service = provider.GetComplexService<IInterface1>();
```

## What Is Difference Between Native `GetService()` and `GetComplexService()`

The native `GetService()` is NOT support:

* generic parameter constraint check
* partially closed generics type

## How To Get `IEnumerable<?>`

the `IComplexServiceFactory` provide a `GetClosedServiceTypes()` API.
