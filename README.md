# LDtk Parser for Monogame

This library is intended to be a [Monogame](https://www.monogame.net/) compatible parser for [LDtk](https://ldtk.io/)'s file format `.ldtk`

## Preface

The approach is to return models that use Monogame types so that they can be digested easily game side.

No rendering is provided as this is not the parser's responsibility.

## Entities

To keep some amount of flexibility `LdtkParser` offers an interface `ILdtkEntity` with one method `FromLdtk(EntityModel entityModel)` to implement

this interface is what is expected when calling `GetEntity<T>`.

## Usage

For now checkout this repo and add `LdtkParser` as a Project reference in Visual Studio

API Documentation : https://michelbartz.github.io/LdtkParser/

**This is still currently WiP the API might change in the future**

## Example

Simple Example
```
var filePath = AppContext.BaseDirectory + Content.RootDirectory + "\\world.ldtk";
world = new World(filePath, GraphicsDevice);

var ldtkLevel = world.GetLevel("Level_1");
var entities = ldtkLevel.GetLayerByName<Entities>("Layer_Name");
var player = entities.GetEntity<Player>();
```

Currently we do have a hard dependency on `GraphicsDevice` to generate `Texture2D` but it is something we want to move away from.

## Roadmap

- Hot reload: The idea would be to have it on (i.e. `new World(file, GraphicsDevice, hotReload=true)`) then when the `.ldtk` file is updated on disk an event is propagated so the engine sitting on top can do refreshes if it wants to. This would enable fast prototyping without having to rebuild.
- Add integration tests that'll simply validate successful parsing of various file outputs.
- Support for the upcoming 0.8