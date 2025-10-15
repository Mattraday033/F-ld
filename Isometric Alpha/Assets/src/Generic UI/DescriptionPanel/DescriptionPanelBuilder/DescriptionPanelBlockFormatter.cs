using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum BlockFormatType {None = 0, PartyMemberStats = 1, CombatHover = 2, PlayerStats = 3}

public class BlockFormat
{
    private static Color combatHoverOutlineGrey = new Color32(155, 155, 155, 255);
    private static Color combatHoverInteriorGrey = new Color32(25, 25, 25, 255);

    private static Color iconBackgroundBlack = Color.black;
    private static Color iconOutlineGrey = Color.white;

    public bool disableImages;

    public Color outlineColor;
    public Color interiorColor;

    public Color iconOutlineColor;
    public Color iconBackgroundColor;

    public Vector2Int iconSizeParams = Vector2Int.zero;
    public int fontsize = -1;
    public int spaceBetweenIconAndText = -1;


    public BlockFormat(bool disableImages)
    {
        this.disableImages = disableImages;

        this.outlineColor = combatHoverOutlineGrey;
        this.interiorColor = combatHoverOutlineGrey;
    }

    public BlockFormat(Color outlineColor, Color interiorColor)
    {
        this.disableImages = false;

        this.outlineColor = outlineColor;
        this.interiorColor = interiorColor;
    }

    public BlockFormat(bool disableImages, Color iconOutlineColor, Color iconBackgroundColor)
    {
        this.disableImages = disableImages;

        this.outlineColor = combatHoverOutlineGrey;
        this.interiorColor = combatHoverOutlineGrey;
        
        this.iconOutlineColor = iconOutlineColor;
        this.iconBackgroundColor = iconBackgroundColor;
    }

    public BlockFormat(Color outlineColor, Color interiorColor, Color iconOutlineColor, Color iconBackgroundColor)
    {
        this.disableImages = false;

        this.outlineColor = outlineColor;
        this.interiorColor = interiorColor;
        
        this.iconOutlineColor = iconOutlineColor;
        this.iconBackgroundColor = iconBackgroundColor;
    }

    public BlockFormat(Color outlineColor, Color interiorColor, Color iconOutlineColor, Color iconBackgroundColor, Vector2Int iconSizeParams, int fontsize, int spaceBetweenIconAndText)
    {
        this.disableImages = false;

        this.outlineColor = outlineColor;
        this.interiorColor = interiorColor;

        this.iconOutlineColor = iconOutlineColor;
        this.iconBackgroundColor = iconBackgroundColor;
        
        this.iconSizeParams = iconSizeParams;
        this.fontsize = fontsize;
        this.spaceBetweenIconAndText = spaceBetweenIconAndText;
    }

    public bool hasSizeParams()
    {
        return !iconSizeParams.Equals(Vector2Int.zero);
    }

    public bool hasFontSizeParams()
    {
        return fontsize >= 0;
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
                return new BlockFormat(Color.clear, Color.clear, Color.clear, Color.black, new Vector2Int(45, 45), 26, 5);

            case BlockFormatType.CombatHover:
                return new BlockFormat(combatHoverOutlineGrey, combatHoverOutlineGrey, Color.clear, Color.black);

            case BlockFormatType.PartyMemberStats:
                return new BlockFormat(false, iconOutlineGrey, iconBackgroundBlack);
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

    public BlockFormat format;

    public void setFormat(BlockFormat format)
    {
        this.format = format;

        if (format.disableImages)
        {
            setImageEnabled(panelOutline, false);
            setImageEnabled(panelInterior, false);
        }
        else
        {
            DescriptionPanel.setImageColor(panelOutline, format.outlineColor);
            DescriptionPanel.setImageColor(panelInterior, format.interiorColor);
        }
    }

    private void setImageEnabled(Image image, bool isEnabled)
    {
        if (image != null)
        {
            image.enabled = isEnabled;
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

    }

}