using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public enum Type { Dirt,Grass,Sand,Water,Stone,Void,ShallowWater,OreStone,SeedGrass,Till};
    public enum Class { Collidable, Ore,Normal, TallGrass,Tillable,Dirtable};
    public Type type;
    public Class tileClass;
    private Type modifiedState;
    public World.WorldClickType requiredClickType;
    public Tile(Type type)
    {
        modifyTileType(type);
        /*this.type = type;
        this.tileClass = getClassFromType(type);
        this.modifiedState = getModifiedStateFromType(type);
        this.requiredClickType = getRequiredClickTypeFromType(type);*/
    }
    public void modifyTileType(Type type)
    {
        this.type = type;
        this.tileClass = getClassFromType(type);
        this.modifiedState = getModifiedStateFromType(type);
        this.requiredClickType = getRequiredClickTypeFromType(type);
    }
    public World.WorldClickType getRequiredClickType()
    {
        return requiredClickType;
    }
    public static World.WorldClickType getRequiredClickTypeFromType(Type type)
    {
        switch(type)
        {
            case Type.Dirt:
                return World.WorldClickType.Till;
            case Type.Grass:
                return World.WorldClickType.Till;
            case Type.OreStone:
                return World.WorldClickType.Mine;
            default:
                return World.WorldClickType.Break;
        }
    }
    public static Type getModifiedStateFromType(Type type)
    {
        switch(type)
        {
            case Tile.Type.SeedGrass:
                return Tile.Type.Grass;
            case Tile.Type.Dirt:
                return Tile.Type.Till;
            case Tile.Type.Grass:
                return Tile.Type.Dirt;
            case Tile.Type.OreStone:
                return Tile.Type.Stone;
            default:
                return type;
        }
    }
    public static Class getClassFromType(Type type)
    {
        switch(type)
        {
            case Tile.Type.Water:
                return Tile.Class.Collidable;
            case Tile.Type.OreStone:
                return Tile.Class.Ore;
            default:
                return Tile.Class.Normal;
        }
    }

}
