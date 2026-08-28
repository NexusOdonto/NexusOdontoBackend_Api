using Domain.Entities.Catalogs;

namespace Domain.Entities.Odontogram;

public class ToothEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Guid DentitionTypeId { get; set; }
    public int Quadrant { get; set; }
    public int Position { get; set; }

    public DentitionTypeEntity? DentitionTypeEntity { get; set; }

    private ToothEntity()
    {
    }

    public ToothEntity(
        string code,
        string name,
        Guid dentitionTypeId,
        int quadrant,
        int position)
    {
        Code = code;
        Name = name;
        DentitionTypeId = dentitionTypeId;
        Quadrant = quadrant;
        Position = position;
    }

    public void Update(string name, Guid dentitionTypeId, int quadrant, int position)
    {
        Name = name;
        DentitionTypeId = dentitionTypeId;
        Quadrant = quadrant;
        Position = position;
    }
}
