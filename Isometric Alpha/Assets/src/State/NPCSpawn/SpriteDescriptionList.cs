using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteDescriptionList
{
    #region Obstacles

    public readonly static SpriteDescription slaveBed = new SpriteDescription(spriteName: PrefabNames.slaveBed, offset: Constants.onTableHeightOffset*-3f);
    public readonly static SpriteDescription slaveBedFlipped = new SpriteDescription(spriteName: PrefabNames.slaveBed, flipX: true, offset: Constants.onTableHeightOffset*-3f);

    public readonly static SpriteDescription shackWallHalf = new SpriteDescription(spriteName: PrefabNames.shackWallHalf);

    public readonly static SpriteDescription emptyPolearmRackFlipped = new SpriteDescription(spriteName: PrefabNames.emptyPolearmRack, flipX: true);
    public readonly static SpriteDescription emptyWeaponTable = new SpriteDescription(spriteName: PrefabNames.emptyWeaponTable);

    public readonly static SpriteDescription squareCratesSmall = new SpriteDescription(spriteName: PrefabNames.squareCratesSmall);
    public readonly static SpriteDescription squareCratesSmallRaised = new SpriteDescription(spriteName: PrefabNames.squareCratesSmall, offset: Constants.onTableHeightOffset*-1);

    public readonly static SpriteDescription spikesDown = new SpriteDescription(spriteName: PrefabNames.spikesDown, tint: Color.white);

    #endregion
    #region Rubble Obstacles

    public readonly static SpriteDescription tutorialRubble = new SpriteDescription(spriteName: PrefabNames.tutorialRubble, useRubbleColor: true);
    public readonly static SpriteDescription southDescendingRubble = new SpriteDescription(spriteName: PrefabNames.southDescendingRubble, useRubbleColor: true);
    public readonly static SpriteDescription southWestDescendingRubble = new SpriteDescription(spriteName: PrefabNames.southWestDescendingRubble, useRubbleColor: true);
    public readonly static SpriteDescription northWestDescendingRubble = new SpriteDescription(spriteName: PrefabNames.northWestDescendingRubble, useRubbleColor: true);
    public readonly static SpriteDescription blockRubble = new SpriteDescription(spriteName: PrefabNames.blockRubble, useRubbleColor: true);
    public readonly static SpriteDescription lowRubble = new SpriteDescription(spriteName: PrefabNames.lowRubble, useRubbleColor: true);

    #endregion
    #region Obstacles With Secret Door Flags

    public readonly static SpriteDescription mineLvl3WallSecretDoor = new SpriteDescription(spriteName: PrefabNames.mineLvl3WallSecretDoor);
    public readonly static SpriteDescription mineLvl3GroundSecretDoor = new SpriteDescription(spriteName: PrefabNames.mineLvl3GroundSecretDoor, sortingLayerInfo: SortingLayerManager.secondSortingLayerInfo);
    public readonly static SpriteDescription manseHalfWallSecretDoor = new SpriteDescription(spriteName: PrefabNames.manseHalfWallSecretDoor);

    #endregion
    #region NPCs

    public readonly static SpriteDescription tripleBarrel = new SpriteDescription(spriteName: PrefabNames.tripleBarrel);
    public readonly static SpriteDescription showtouch = new SpriteDescription(spriteName: PrefabNames.showtouch);
    public readonly static SpriteDescription leafPile = new SpriteDescription(spriteName: PrefabNames.leafPile);
    public readonly static SpriteDescription directorStatue = new SpriteDescription(spriteName: PrefabNames.directorStatuePath, offset: -.1f);
    public readonly static SpriteDescription brokenDirectorStatue = new SpriteDescription(spriteName: PrefabNames.brokenDirectorStatuePath, offset: -.1f);
    public readonly static SpriteDescription controlPanelFlipped = new SpriteDescription(spriteName: PrefabNames.controlPanel, flipX: Constants.flipX, offset: Constants.onTableHeightOffset*2);
    public readonly static SpriteDescription lowStalagmite = new SpriteDescription(spriteName: PrefabNames.lowStalagmite);

    #endregion
    #region Ladders

    public readonly static SpriteDescription ladderTallSW = new SpriteDescription(spriteName: PrefabNames.ladderTallSW, offset: LadderSpawnDetails.offsetY);
    public readonly static SpriteDescription ladderTallSWFlipped = new SpriteDescription(spriteName: PrefabNames.ladderTallSW, flipX: Constants.flipX, offset: LadderSpawnDetails.offsetY);
    public readonly static SpriteDescription ladderTallNE = new SpriteDescription(spriteName: PrefabNames.ladderTallNE, offset: LadderSpawnDetails.offsetY);
    public readonly static SpriteDescription ladderTallNEFirstSortingLayer = new SpriteDescription(spriteName: PrefabNames.ladderTallNE, sortingLayerInfo: SortingLayerManager.firstSortingLayerInfo, offset: LadderSpawnDetails.offsetY);
    public readonly static SpriteDescription ladderShortNE = new SpriteDescription(spriteName: PrefabNames.ladderShortNE, offset: LadderSpawnDetails.offsetY);
    public readonly static SpriteDescription ladderShortNEGroundSortingLayer = new SpriteDescription(spriteName: PrefabNames.ladderShortNE, sortingLayerInfo: SortingLayerManager.groundSortingLayerInfo, offset: .70f);

    #endregion
    #region Vaultable Objects

    public readonly static SpriteDescription vaultableBarrels = new SpriteDescription(spriteName: PrefabNames.vaultableBarrels);
    public readonly static SpriteDescription vaultableRocks = new SpriteDescription(spriteName: PrefabNames.vaultableRocks, useRubbleColor: true);
    public readonly static SpriteDescription destroyableBarricade = new SpriteDescription(spriteName: PrefabNames.destroyableBarricade);

    public readonly static SpriteDescription stoneVaultableGap = new SpriteDescription(spriteName: PrefabNames.stoneVaultableGap, sortingLayerInfo: SortingLayerManager.groundSortingLayerInfo, offset: Constants.onTableHeightOffset*10);
    public readonly static SpriteDescription lavaVaultableGapHalfGroundSortingLayer = new SpriteDescription(spriteName: PrefabNames.lavaVaultableGapHalf, sortingLayerInfo: SortingLayerManager.groundSortingLayerInfo, offset: Constants.onTableHeightOffset);
    public readonly static SpriteDescription lavaVaultableGapHalfButtonSortingLayer = new SpriteDescription(spriteName: PrefabNames.lavaVaultableGapHalf, sortingLayerInfo: SortingLayerManager.buttonSortingLayerInfo, offset: Constants.onTableHeightOffset);

    #endregion
    #region Gates

    // Gate rubble is tinted white rather than rubble coloured, matching the gates' old useRubbleColor: false
    public readonly static SpriteDescription blockRubbleGate = new SpriteDescription(spriteName: PrefabNames.blockRubble);
    public readonly static SpriteDescription lowRubbleGate = new SpriteDescription(spriteName: PrefabNames.lowRubble);

    // Portcullises are flipped on the descending X axis only
    public readonly static SpriteDescription portcullis1x1 = new SpriteDescription(spriteName: PrefabNames.portcullis1x1Path);

    public readonly static SpriteDescription portcullis2x1 = new SpriteDescription(spriteName: PrefabNames.portcullis2x1Path);
    public readonly static SpriteDescription portcullis2x1Flipped = new SpriteDescription(spriteName: PrefabNames.portcullis2x1Path, flipX: Constants.flipX);

    public readonly static SpriteDescription portcullis3x1 = new SpriteDescription(spriteName: PrefabNames.portcullis3x1Path, offset: Constants.onTableHeightOffset*5);
    public readonly static SpriteDescription portcullis3x1Flipped = new SpriteDescription(spriteName: PrefabNames.portcullis3x1Path, flipX: Constants.flipX, offset: Constants.onTableHeightOffset*5);

    #endregion
    #region Books

    public readonly static SpriteDescription note = new SpriteDescription(spriteName: PrefabNames.note, offset: Constants.onTableHeightOffset*2);
    public readonly static SpriteDescription noteOnGround = new SpriteDescription(spriteName: PrefabNames.note, offset: Constants.onGroundHeightOffset);

    #endregion
}
