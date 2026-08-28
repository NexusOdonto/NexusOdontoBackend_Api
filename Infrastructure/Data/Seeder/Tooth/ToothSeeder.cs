using Infrastructure.Data.Seeder.DentitionType;
using ToothEntity = Domain.Entities.Odontogram.ToothEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder.Tooth;

public static class ToothSeeder
{
    //Notacion FDI: el primer digito es el cuadrante y el segundo la posicion.
    //Permanentes 11-18, 21-28, 31-38, 41-48 (32 piezas).
    //Temporales  51-55, 61-65, 71-75, 81-85 (20 piezas).
    private static readonly string[] QuadrantNames =
    [
        "Upper Right", "Upper Left", "Lower Left", "Lower Right"
    ];

    private static readonly string[] PermanentPositions =
    [
        "Central Incisor", "Lateral Incisor", "Canine", "First Premolar",
        "Second Premolar", "First Molar", "Second Molar", "Third Molar"
    ];

    private static readonly string[] DeciduousPositions =
    [
        "Central Incisor", "Lateral Incisor", "Canine", "First Molar", "Second Molar"
    ];

    public static void Seed(ModelBuilder modelBuilder)
    {
        var data = new List<object>();

        //Permanentes: cuadrantes 1 a 4, posiciones 1 a 8
        for (var quadrant = 1; quadrant <= 4; quadrant++)
        {
            for (var position = 1; position <= PermanentPositions.Length; position++)
            {
                data.Add(new
                {
                    Code = $"{quadrant}{position}",
                    Name = $"{QuadrantNames[quadrant - 1]} {PermanentPositions[position - 1]}",
                    DentitionTypeId = DentitionTypeSeeder.Permanent,
                    Quadrant = quadrant,
                    Position = position
                });
            }
        }

        //Temporales: cuadrantes 5 a 8, posiciones 1 a 5
        for (var quadrant = 5; quadrant <= 8; quadrant++)
        {
            for (var position = 1; position <= DeciduousPositions.Length; position++)
            {
                data.Add(new
                {
                    Code = $"{quadrant}{position}",
                    Name = $"Primary {QuadrantNames[quadrant - 5]} {DeciduousPositions[position - 1]}",
                    DentitionTypeId = DentitionTypeSeeder.Deciduous,
                    Quadrant = quadrant,
                    Position = position
                });
            }
        }

        modelBuilder.Entity<ToothEntity>().HasData(data);
    }
}
