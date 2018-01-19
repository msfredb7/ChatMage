public partial class DataSaver
{
    // NB: Le Enum doit match avec les categories
    public enum Type
    {
        Levels = 0,
        Loadout = 1,
        Account = 2,
        Armory = 3,
        Tutorial = 4,
        Dialog = 5
    }
    private Category[] categories = new Category[]
    {
        new Category("levelSelect.dat"),
        new Category("loadout.dat"),
        new Category("account.dat"),
        new Category("armory.dat"),
        new Category("tutorial.dat"),
        new Category("dialog.dat")
    };
}