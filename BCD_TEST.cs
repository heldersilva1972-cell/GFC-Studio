// Quick test of BCD timestamp parsing
// Add this to Program.cs temporarily, right before app.Run()

var testHex = "20 26 01 09 15 11 03"; // From Wireshark: 2026-01-09 15:11:03
var testBytes = testHex.Split(' ').Select(s => Convert.ToByte(s, 16)).ToArray();

Console.WriteLine("=== BCD TIMESTAMP TEST ===");
Console.WriteLine($"Input bytes: {testHex}");

// Call the parser (you'll need to make TryParseBcdDateTime public temporarily)
// var result = WgResponseParser.TryParseBcdDateTime(testBytes, DateTimeKind.Utc);

// For now, manually decode:
int DecodeBcd(byte bcd)
{
    if ((bcd & 0x0F) > 0x09 || (bcd >> 4) > 0x09) return 0;
    return ((bcd >> 4) * 10) + (bcd & 0x0F);
}

var century = DecodeBcd(testBytes[0]);
var year = DecodeBcd(testBytes[1]);
var month = DecodeBcd(testBytes[2]);
var day = DecodeBcd(testBytes[3]);
var hour = DecodeBcd(testBytes[4]);
var minute = DecodeBcd(testBytes[5]);
var second = DecodeBcd(testBytes[6]);

Console.WriteLine($"Decoded: {century * 100 + year}-{month:D2}-{day:D2} {hour:D2}:{minute:D2}:{second:D2}");
Console.WriteLine($"Expected: 2026-01-09 15:11:03");
Console.WriteLine("=========================");
