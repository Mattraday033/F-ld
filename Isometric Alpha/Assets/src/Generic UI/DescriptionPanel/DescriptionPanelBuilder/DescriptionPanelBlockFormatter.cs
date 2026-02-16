using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum BlockFormatType {None = 0, PartyMemberStats = 1, CombatHover = 2, PlayerStats = 3, CombatResults = 4}

public class BlockFormat
{
    public Color ?fontColor;

    public Color iconOutlineColor;
    public Color iconBackgroundColor;

    public Vector2Int iconSizeParams = Vector2Int.zero;
    public int fontSize = -1;
    public int spaceBetweenIconAndText = -1;


    public BlockFormat(Color iconOutlineColor, Color iconBackgroundColor, Color ?fontColor = null, Vector2Int iconSizeParams = new Vector2Int(), int fontSize = -1, int spaceBetweenIconAndText = -1)
    {
        if(fontColor == null)
        {
            this.fontColor = ColorList.grey25;
        } else
        {
            this.fontColor = fontColor;
        }

        this.iconOutlineColor = iconOutlineColor;
        this.iconBackgroundColor = iconBackgroundColor;

        this.iconSizeParams = iconSizeParams;
        this.fontSize = fontSize;
        this.spaceBetweenIconAndText = spaceBetweenIconAndText;
    }

    public bool hasSizeParams()
    {
        return !iconSizeParams.Equals(Vector2Int.zero);
    }

    public bool hasFontSizeParams()
    {
        return fontSize >= 0;
    }

    public bool hasSpacingSizeParams()
    {
        return spaceBetweenIconAndText >= 0;
    }

    public static BlockFormat getBlockFormat(BlockFormatType type)
    {
        switch (type)
        {
            case BlockFormatType.PlayerStats:
                return new BlockFormat(ColorList.darkUICyan, ColorList.grey25, iconSizeParams: new Vector2Int(45, 45), fontSize: 26, spaceBetweenIconAndText: 5);

            case BlockFormatType.CombatHover:
                return new BlockFormat(ColorList.darkUICyan, ColorList.grey25, ColorList.grey245);

            case BlockFormatType.PartyMemberStats: 
                return new BlockFormat(ColorList.darkUICyan, ColorList.grey25);

            case BlockFormatType.CombatResults: 
                return new BlockFormat(ColorList.darkUICyan, ColorList.grey25);
            default:
                return null;
        }
    }

}

public class DescriptionPanelBlockFormatter : MonoBehaviour
{
    public bool preventPlusButtons = true;
    public bool flipDirection = false;

    public Image panelOutline;
    public Image panelInterior;

    public Transform rowParent;

    public BlockFormatType formatOverride = BlockFormatType.None;
    public BlockFormat format;

    public bool centerAllText = false;

    public void setFormat(BlockFormat format)
    {
        if(formatOverride == BlockFormatType.None)
        {
            this.format = format;
        } else
        {
            this.format = BlockFormat.getBlockFormat(formatOverride);
        }
    }

    public void applyFormat(DescriptionPanelRow row)
    {
        if (format == null)
        {
            return;
        }

        row.setIconBackgroundColor(format.iconBackgroundColor);
        row.setIconOutlineColor(format.iconOutlineColor);

        if (preventPlusButtons && row.plusButton != null)
        {
            Destroy(row.plusButton);
            row.plusButton = null;
        }

        if (flipDirection)
        {
            row.flipDirection();
        }

        if(centerAllText)
        {
            row.centerText();
        }

        row.setDescriptionTextColor(format.fontColor);

    }

}