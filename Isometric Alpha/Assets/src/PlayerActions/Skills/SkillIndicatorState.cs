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

        frontSelectorTwoColor = indicator.frontSelectorTwo.spriteRenderer.color;
        backSelectorTwoColor = indicator.backSelectorTwo.spriteRenderer.color;
    }

    public void restore(SkillIndicator indicator)
    {
        indicator.setColor(color);

        indicator.frontSelector.color = frontSelectorColor;
        indicator.backSelector.color = backSelectorColor;

        indicator.frontSelectorTwo.spriteRenderer.color = frontSelectorTwoColor;
        indicator.backSelectorTwo.spriteRenderer.color = backSelectorTwoColor;
    }
}
