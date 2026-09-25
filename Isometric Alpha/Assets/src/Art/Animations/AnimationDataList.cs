using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationDataList
{
    private readonly static Sprite blank = Resources.Load<Sprite>(PrefabNames.abilityEffectFolderPath + "Blank");

    public readonly static AnimationData frontSelector2 = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = Resources.LoadAll<Sprite>(PrefabNames.abilityEffectFolderPath + EffectAnimationType.FrontSelector2)
            // loadOrderedSprites(PrefabNames.abilityEffectFolderPath + EffectAnimationType.FrontSelector2)
        },
        new float[] { 2f/15f, 2f/15f, 2f/15f, 2f/15f, 2f/15f, 2f/15f, 2f/15f, 2f/15f,
                      2f/15f, 2f/15f, 2f/15f, 2f/15f, 2f/15f, 2f/15f, 2f/15f, 1f/60f });

    public readonly static AnimationData backSelector2 = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = Resources.LoadAll<Sprite>(PrefabNames.abilityEffectFolderPath + EffectAnimationType.BackSelector2)
            // loadOrderedSprites(PrefabNames.abilityEffectFolderPath + EffectAnimationType.BackSelector2)
        },
        new float[] { 2f/15f, 2f/15f, 2f/15f, 2f/15f, 2f/15f, 2f/15f, 2f/15f, 2f/15f,
                      2f/15f, 2f/15f, 2f/15f, 2f/15f, 2f/15f, 2f/15f, 2f/15f, 1f/60f });

    public readonly static AnimationData slash = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                blank,
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_67"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_66"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_65"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_64"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_63"),
                blank,
                blank
            }
        },
        new float[] { 50f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 1f/60f });

    public readonly static AnimationData blunt = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                blank,
                getSprite(PrefabNames.abilityEffectFolderPath + "Blunt", "Blunt_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Blunt", "Blunt_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Blunt", "Blunt_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Blunt", "Blunt_3"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Blunt", "Blunt_4"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Blunt", "Blunt_5"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Blunt", "Blunt_6"),
                blank
            }
        },
        new float[] { 50f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f,
                      1f/60f });

    public readonly static AnimationData pierce = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                blank,
                getSprite(PrefabNames.abilityEffectFolderPath + "Pierce", "Retro Impact Effect Pack 2 A_54"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Pierce", "Retro Impact Effect Pack 2 A_55"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Pierce", "Retro Impact Effect Pack 2 A_56"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Pierce", "Retro Impact Effect Pack 2 A_57"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Pierce", "Retro Impact Effect Pack 2 A_58"),
                blank
            }
        },
        new float[] { 50f/60f, 8f/60f, 8f/60f, 8f/60f, 8f/60f, 8f/60f, 1f/60f });

    public readonly static AnimationData positive = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                blank,
                getSprite(PrefabNames.abilityEffectFolderPath + "Positive", "Positive_24"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Positive", "Positive_25"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Positive", "Positive_26"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Positive", "Positive_27"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Positive", "Positive_28"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Positive", "Positive_29"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Positive", "Positive_30"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Positive", "Positive_31"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Positive", "Positive_32"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Positive", "Positive_33"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Positive", "Positive_34"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Positive", "Positive_35"),
                blank
            }
        },
        new float[] { 50f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f,
                      6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 1f/60f });

    public readonly static AnimationData negative = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                blank,
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_16"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_17"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_18"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_19"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_20"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_21"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_22"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_23"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_24"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_25"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_26"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_27"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_28"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_29"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_30"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Negative", "1761_31"),
                blank
            }
        },
        new float[] { 50f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f,
                      4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f,
                      4f/60f, 1f/60f });

    public readonly static AnimationData healing = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                blank,
                getSprite(PrefabNames.abilityEffectFolderPath + "Healing", "846_33"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Healing", "846_34"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Healing", "846_35"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Healing", "846_36"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Healing", "846_37"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Healing", "846_38"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Healing", "846_39"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Healing", "846_40"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Healing", "846_41"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Healing", "846_42"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Healing", "846_43"),
                blank
            }
        },
        new float[] { 50f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f,
                      4f/60f, 4f/60f, 4f/60f, 4f/60f, 1f/60f });

    public readonly static AnimationData batSwarm = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                blank,
                blank,
                getSprite("Sprites/Characters/Bats/Bat Swarm/Spawn_Front", "Spawn_Front_3"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Spawn_Front", "Spawn_Front_4"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Spawn_Front", "Spawn_Front_5"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Spawn_Front", "Spawn_Front_6"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Spawn_Front", "Spawn_Front_7"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Spawn_Front", "Spawn_Front_8"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Spawn_Front", "Spawn_Front_9"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Spawn_Front", "Spawn_Front_10"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Spawn_Front", "Spawn_Front_11"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Spawn_Front", "Spawn_Front_12"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Attack_Normal_Front", "Attack_Normal_Front_1"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Attack_Normal_Front", "Attack_Normal_Front_2"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Attack_Normal_Front", "Attack_Normal_Front_3"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Attack_Normal_Front", "Attack_Normal_Front_4"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Attack_Normal_Front", "Attack_Normal_Front_5"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Attack_Normal_Front", "Attack_Normal_Front_6"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Attack_Normal_Front", "Attack_Normal_Front_7"),
                getSprite("Sprites/Characters/Bats/Bat Swarm/Attack_Normal_Front", "Attack_Normal_Front_8")
            }
        },
        new float[] { 52f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f,
                      4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f, 4f/60f,
                      4f/60f, 4f/60f, 4f/60f, 1f/60f });

    public readonly static AnimationData acid = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                blank,
                getSprite(PrefabNames.abilityEffectFolderPath + "Acid", "Acid_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Acid", "Acid_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Acid", "Acid_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Acid", "Acid_3"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Acid", "Acid_4"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Acid", "Acid_5"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Acid", "Acid_5")
            }
        },
        new float[] { 50f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 15f/60f, 1f/60f });

    public readonly static AnimationData smokeBomb = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                getSprite(PrefabNames.abilityEffectFolderPath + "SmokeBomb", "SmokeBomb_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "SmokeBomb", "SmokeBomb_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "SmokeBomb", "SmokeBomb_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "SmokeBomb", "SmokeBomb_3"),
                getSprite(PrefabNames.abilityEffectFolderPath + "SmokeBomb", "SmokeBomb_4"),
                getSprite(PrefabNames.abilityEffectFolderPath + "SmokeBomb", "SmokeBomb_5"),
                getSprite(PrefabNames.abilityEffectFolderPath + "SmokeBomb", "SmokeBomb_6"),
                getSprite(PrefabNames.abilityEffectFolderPath + "SmokeBomb", "SmokeBomb_7"),
                getSprite(PrefabNames.abilityEffectFolderPath + "SmokeBomb", "SmokeBomb_8"),
                getSprite(PrefabNames.abilityEffectFolderPath + "SmokeBomb", "SmokeBomb_9"),
                getSprite(PrefabNames.abilityEffectFolderPath + "SmokeBomb", "SmokeBomb_10")
            }
        },
        new float[] { 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f,
                      6f/60f, 6f/60f, 1f/60f });

    public readonly static AnimationData intimidate = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Slash", "Retro Impact Effect Pack 2 A_2")
            }
        },
        new float[] { 5f/60f, 6f/60f, 5f/60f, 6f/60f, 5f/60f, 6f/60f, 5f/60f, 6f/60f,
                      5f/60f, 6f/60f, 5f/60f, 1f/60f });

    public readonly static AnimationData blastingJelly = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                blank,
                getSprite(PrefabNames.abilityEffectFolderPath + "ItchSpritesheetSPELLS1 64x64", "ItchSpritesheetSPELLS1 64x64_89"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_3"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_4"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_5"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_6"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_7"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_8"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_9"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_10"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_11"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_12"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_13"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_14"),
                getSprite(PrefabNames.abilityEffectFolderPath + "explosion (10)", "explosion (10)_15"),
                blank,
                blank
            }
        },
        new float[] { 20f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f,
                      5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f,
                      5f/60f, 5f/60f, 15f/60f, 1f/60f });

    public readonly static AnimationData frontLvlUp = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_3"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_4"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_5"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_6"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_7"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_8"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_9"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_10"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_11"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_12"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_13"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_14"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_15"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_16"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_17"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_18"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_19"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontLvlUp", "Front_20")
            }
        },
        new float[] { 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f,
                      6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f,
                      6f/60f, 6f/60f, 6f/60f, 6f/60f, 1f/60f });

    public readonly static AnimationData backLvlUp = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_3"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_4"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_5"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_6"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_7"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_8"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_9"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_10"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_11"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_12"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_13"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_14"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_15"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_16"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_17"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_18"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_19"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackLvlUp", "Back_20")
            }
        },
        new float[] { 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f,
                      6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f,
                      6f/60f, 6f/60f, 6f/60f, 6f/60f, 1f/60f });

    public readonly static AnimationData transitionIndicator = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_3"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_4"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_5"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_6"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_7"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_8"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_9"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_10"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_11"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_12"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_13"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_14"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_15"),
                getSprite(PrefabNames.abilityEffectFolderPath + "TransitionIndicator", "IMG_5329_16")
            }
        },
        new float[] { 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f,
                      6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f, 6f/60f,
                      1f/60f });

    public readonly static AnimationData gem = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                getSprite(PrefabNames.abilityEffectFolderPath + "Gem", "Selector Gem_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Gem", "Selector Gem_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Gem", "Selector Gem_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Gem", "Selector Gem_3"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Gem", "Selector Gem_0")
            }
        },
        new float[] { 26f/60f, 26f/60f, 27f/60f, 26f/60f, 1f/60f });

    public readonly static AnimationData frontSelector = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontSelector", "Front Selector_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontSelector", "Front Selector_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontSelector", "Front Selector_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontSelector", "Front Selector_3"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontSelector", "Front Selector_4"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontSelector", "Front Selector_5"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontSelector", "FrontSelector_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontSelector", "FrontSelector_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontSelector", "FrontSelector_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontSelector", "Front Selector_6"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontSelector", "Front Selector_7"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontSelector", "Front Selector_8"),
                getSprite(PrefabNames.abilityEffectFolderPath + "FrontSelector", "Front Selector_0")
            }
        },
        new float[] { 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f,
                      10f/60f, 10f/60f, 10f/60f, 10f/60f, 1f/60f });

    public readonly static AnimationData backSelector = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                getSprite(PrefabNames.abilityEffectFolderPath + "BackSelector", "Back Selector_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackSelector", "Back Selector_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackSelector", "Back Selector_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackSelector", "Back Selector_3"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackSelector", "Back Selector_4"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackSelector", "Back Selector_5"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackSelector", "Back Selector_6"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackSelector", "Back Selector_7"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackSelector", "Back Selector_8"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackSelector", "Back Selector_9"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackSelector", "Back Selector_10"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackSelector", "Back Selector_11"),
                getSprite(PrefabNames.abilityEffectFolderPath + "BackSelector", "Back Selector_0")
            }
        },
        new float[] { 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f,
                      10f/60f, 10f/60f, 11f/60f, 9f/60f, 1f/60f });

    public readonly static AnimationData bubbles = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                getSprite(PrefabNames.abilityEffectFolderPath + "Bubbles", "Bubbles_6"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Bubbles", "Bubbles_7"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Bubbles", "Bubbles_8"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Bubbles", "Bubbles_9"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Bubbles", "Bubbles_10"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Bubbles", "Bubbles_11"),
                blank,
                blank,
                blank,
                blank,
                blank,
                blank,
                blank,
                getSprite(PrefabNames.abilityEffectFolderPath + "Bubbles", "Bubbles_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Bubbles", "Bubbles_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Bubbles", "Bubbles_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Bubbles", "Bubbles_3"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Bubbles", "Bubbles_4"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Bubbles", "Bubbles_5")
            }
        },
        new float[] { 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f,
                      10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f, 10f/60f,
                      10f/60f, 10f/60f, 1f/60f });

    public readonly static AnimationData splash = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_3"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_4"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_5"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_6"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_7"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_8"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_9"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_10"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_11"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_12"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_13"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_14"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_15"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_16"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_17"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_18"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_19"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_20"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_21"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_22"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_23"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_24"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Splash", "Splash_25"),
                blank,
                blank
            }
        },
        new float[] { 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f,
                      5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f,
                      5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f, 5f/60f,
                      5f/60f, 5f/60f, 20f/60f, 1f/60f });

    public readonly static AnimationData confused = new AnimationData(
        new Dictionary<SpriteLayer, Sprite[]>()
        {
            [SpriteLayer.Body] = new Sprite[]
            {
                getSprite(PrefabNames.abilityEffectFolderPath + "Confused", "Confused_0"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Confused", "Confused_1"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Confused", "Confused_2"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Confused", "Confused_3"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Confused", "Confused_4"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Confused", "Confused_5"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Confused", "Confused_6"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Confused", "Confused_7"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Confused", "Confused_8"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Confused", "Confused_9"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Confused", "Confused_10"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Confused", "Confused_11"),
                getSprite(PrefabNames.abilityEffectFolderPath + "Confused", "Confused_0")
            }
        },
        new float[] { 10f/60f, 10f/60f, 10f/60f, 9f/60f, 11f/60f, 10f/60f, 10f/60f, 10f/60f,
                      10f/60f, 10f/60f, 10f/60f, 10f/60f, 1f/60f });

    private static Sprite getSprite(string sheetPath, string spriteName)
    {
        foreach(Sprite sprite in Resources.LoadAll<Sprite>(sheetPath))
        {
            if(sprite.name == spriteName)
            {
                return sprite;
            }
        }

        Debug.LogWarning("AnimationDataList: sprite " + spriteName + " not found in " + sheetPath);
        return null;
    }

    // private static Sprite[] loadOrderedSprites(string path)
    // {
    //     Sprite[] sprites = Resources.LoadAll<Sprite>(path);

    //     Array.Sort(sprites, (a, b) => getSpriteIndex(a).CompareTo(getSpriteIndex(b)));

    //     return sprites;
    // }

    // private static int getSpriteIndex(Sprite sprite)
    // {
    //     return int.Parse(sprite.name.Substring(sprite.name.LastIndexOf('_') + 1));
    // }
}
