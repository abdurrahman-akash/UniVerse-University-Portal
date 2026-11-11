namespace SchoolManagementSystem.Helpers
{
    // Small helpers used by the Razor views (avatar initials and colours).
    public static class UiHelpers
    {
        private static readonly string[] Tiles = { "tile-blue", "tile-green", "tile-amber", "tile-violet", "tile-rose" };

        // "Abdur Rahman" -> "AR"
        public static string Initials(string? firstName, string? lastName)
        {
            var first = string.IsNullOrWhiteSpace(firstName) ? "" : firstName.Trim()[..1];
            var last = string.IsNullOrWhiteSpace(lastName) ? "" : lastName.Trim()[..1];
            var result = (first + last).ToUpperInvariant();
            return result.Length == 0 ? "?" : result;
        }

        // The same text always gets the same colour, so a person's avatar never changes colour.
        public static string TileFor(string? text)
        {
            var sum = (text ?? "").Sum(c => c);
            return Tiles[sum % Tiles.Length];
        }
    }
}
