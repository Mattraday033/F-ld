using UnityEngine;

//anything that draws a character's sprite layers with the ColorReplace shader, such as a sprite renderer list or a headshot
public interface IColorSchemaTarget
{
    //the material that draws this layer, or null when the target has no renderer or image of its own for it
    Material getColorSchemaMaterial(SpriteLayer layer);

    //called once after the shared code below has changed this target's materials
    void onColorSchemaMaterialsChanged();
}

public static class ColorSchemaTargetExtensions
{
    private const string replaceVarName = "_Replace";

    public static void interpretSchema(this IColorSchemaTarget target, ColorReplaceSchema schema)
    {
        foreach(SpriteLayer layer in EnumUtil.CharacterSpriteLayers)
        {
            Material material = target.getColorSchemaMaterial(layer);

            if(material == null)
            {
                continue;
            }

            foreach(ColorReplacementSlot slot in EnumUtil.ColorReplacementSlots)
            {
                material.SetColor(slot.getMaterialVARName(), schema.getColor(layer, slot));
            }
        }

        target.onColorSchemaMaterialsChanged();
    }

    public static void ignoreColorReplace(this IColorSchemaTarget target)
    {
        foreach(SpriteLayer layer in EnumUtil.CharacterSpriteLayers)
        {
            Material material = target.getColorSchemaMaterial(layer);

            if(material != null)
            {
                material.SetFloat(replaceVarName, Constants.falseFloatToBool);
            }
        }

        target.onColorSchemaMaterialsChanged();
    }
}
