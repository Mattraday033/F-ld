using UnityEngine;

public class SkillIndicatorState
{
    private readonly Color color;

    private readonly Color frontSelectorColor;
    private readonly Color backSelectorColor;

    private readonly Color frontSelectorTwoColor;
    private readonly Color backSelectorTwoColor;

    public SkillIndicatorState(SkillIndicator indicator)
    {
        color = indicator.getColor();

        frontSelectorColor = indicator.frontSelector.color;
        backSelectorColor = indicator.backSelector.color;

        frontSelectorTwoColor = indicator.getSelectorTwoColor(indicator.frontSelectorTwo);
        backSelectorTwoColor = indicator.getSelectorTwoColor(indicator.backSelectorTwo);
    }

    public void restore(SkillIndicator indicator)
    {
        indicator.setColor(color);

        indicator.frontSelector.color = frontSelectorColor;
        indicator.backSelector.color = backSelectorColor;

        indicator.setSelectorTwoColor(indicator.frontSelectorTwo, frontSelectorTwoColor);
        indicator.setSelectorTwoColor(indicator.backSelectorTwo, backSelectorTwoColor);
    }
}
