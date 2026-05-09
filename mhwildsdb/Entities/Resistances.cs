namespace mhwildsdb.Entities;

public class Resistances
{
    public int Fire { get; set; }
    public int Water { get; set; }
    public int Ice { get; set; }
    public int Thunder { get; set; }
    public int Dragon { get; set; }

    private Resistances() { }

    public Resistances(int fire, int water, int ice, int thunder, int dragon)
    {
        Fire = fire;
        Water = water;
        Ice = ice;
        Thunder = thunder;
        Dragon = dragon;
    }
}   
