# Kinvey Support for Windows Phone 8 #

This project aims to provide support to Windows Phone 8 apps backed by [Kinvey BaaS services](http://www.kinvey.com/).

## Dependencies ##

- [Microsoft BCL Build Components](http://nuget.org/packages/Microsoft.Bcl.Build/)
- [Microsoft HTTP Client Libraries](http://nuget.org/packages/Microsoft.Net.Http/2.1.10)
- [Json.NET](http://nuget.org/packages/Newtonsoft.Json/)

These dependencies will be downloaded and installed by Visual Studio during the build process if your setup allows.

## Building the DLL ##

You can build the solution file (Kinvey.sln) with Visual Studio 2012 in release mode either through Visual Studio or through the command line...

    .../devenv.com Kinvey.sln /rebuild release

## Running the Tests ##

A suite of N-Unit tests is provided in the Com.Penrillian.Kinvey.Test project which can be run using any test runner.

## Example Usage ##

### Initialising the DLL ###

This **MUST** be done before the DLL is used by your app. It is best practice to do this during app initialization. Inform the KinveySettings class of your AuthToken and AuthKey...

```c#
    KinveySettings.Get().AppAuthToken = "h4gs56j4t8a4n1a3j4t8a34j5t34a534fgag23bcxvz=";
    KinveySettings.Get().AppKey = "MyApp";
```

### Basic CRUD ###

Accessing collections is done using the 
`IKinveyService <T>` interface. The type parameter `T` should be a type marked with the `KinveyCollection` attribute, using the collection name as the parameter. Serialization attributes can be added to the properties of this class to match your collection schema...

```c#
    [KinveyCollection("giraffe")]
    public class Giraffe : KinveyObject
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }
    }
```

You can get an instance of an IKinveyService from the KinveyFactory class...

```c#
    var service = KinveyFactory.GetService<Giraffe>();
```

You can use this service object to perform TAP operations on your data...

```c#
	// Creating
    var giraffe = await service.Create(new Giraffe {Name = "Dave", Age = 42});
	// Reading
    giraffe = await service.Read(giraffe.Id);
	// Updating
    giraffe = await service.Update(giraffe);
	// Deleting
	await service.Delete(giraffe);
```

### Querying ###

You can count, query and delete by query all as TAP operations...

```c#
	var giraffeCount = await dataService.Count();
	var daveCount = await dataService.Count(new KinveyQuery<Giraffe>().Constrain(g => g.Name, "Dave"));
	var daves = await dataService.Read(new KinveyQuery<Giraffe>().Constrain(g => g.Name, "Dave"));
	var deletedDaveCount = await dataService.Delete(new KinveyQuery<Giraffe>().Constrain(g => g.Name, "Dave"));
```

#### Working with Constraints ####

Query constraint objects give a fluid API for defining queries. 

```c#
	var query = new KinveyQuery<Giraffe>();
	// constrain
	query = query.Constrain(g => g.Name, "Dave");
	// comparison constraints
	query = query.Constrain(g => g.Age, Is.GreaterThan(18));
	// removing constraints
	query = query.Release(g => g.Name);
	// custom comparison constraints
	// see: http://docs.mongodb.org/manual/reference/operator/
	query = query.Constrain(g => g.Name, Is.It("$ne", "stephen"));
	// paging
	query = query.Limit(20); // <- page 1
	query = query.Skip(20); // <- page 2
	// chaining
	query = new KinveyQuery<Giraffe>()
				.Constrain(g => g.Name, "Dave")
				.Constrain(g => g.Age, Is.GreaterThan(18))
				.Limit(20)
				.Skip(20);
```