using Ludo.Game.Enums;

public class PlayerSetupModel
{
    public string Name { get; set; } = "";
    public Color? Color { get; set; }

    public bool IsNameValid => !string.IsNullOrWhiteSpace(Name);
    public bool IsColorValid => Color != null;
}
