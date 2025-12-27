// [NEW]
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GFC.BlazorServer.Data.Migrations
{
    public partial class AddDynamicFormTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DynamicForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SchemaJson = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicForms", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DynamicForms_Name",
                table: "DynamicForms",
                column: "Name",
                unique: true);

            migrationBuilder.InsertData(
                table: "DynamicForms",
                columns: new[] { "Name", "SchemaJson" },
                values: new object[,]
                {
                    {
                        "hall-rental",
                        @"{
                          ""Name"": ""hall-rental"",
                          ""Fields"": [
                            {
                              ""Section"": ""Applicant & Membership"",
                              ""Fields"": [
                                { ""Name"": ""name"", ""Label"": ""Full Name"", ""Type"": ""text"", ""Required"": true, ""GridCols"": 1 },
                                { ""Name"": ""memberStatus"", ""Label"": ""Membership Status"", ""Type"": ""select"", ""Required"": true, ""Options"": [""Non-Member"", ""Member"", ""Non-Profit Organization""], ""GridCols"": 1 },
                                { ""Name"": ""sponsoringMember"", ""Label"": ""Sponsoring Member"", ""Type"": ""text"", ""Required"": true, ""GridCols"": 2, ""Conditional"": { ""Field"": ""memberStatus"", ""Value"": ""Member"" } },
                                { ""Name"": ""dob"", ""Label"": ""Date of Birth (Must be 21+)"", ""Type"": ""date-split"", ""Required"": true, ""GridCols"": 2 },
                                { ""Name"": ""address"", ""Label"": ""Street Address"", ""Type"": ""text"", ""Required"": true, ""GridCols"": 2 },
                                { ""Name"": ""city"", ""Label"": ""City"", ""Type"": ""text"", ""Required"": true, ""GridCols"": 1 },
                                { ""Name"": ""zip"", ""Label"": ""Zip Code"", ""Type"": ""text"", ""Required"": true, ""GridCols"": 1 },
                                { ""Name"": ""phone"", ""Label"": ""Phone Number"", ""Type"": ""tel"", ""Required"": true, ""GridCols"": 1 },
                                { ""Name"": ""email"", ""Label"": ""Email Address"", ""Type"": ""email"", ""Required"": true, ""GridCols"": 1 }
                              ]
                            },
                            {
                              ""Section"": ""Event Details"",
                              ""Fields"": [
                                { ""Name"": ""eventType"", ""Label"": ""Type of Function"", ""Type"": ""select"", ""Required"": true, ""Options"": [""Wedding"", ""Birthday Party"", ""Baby Shower"", ""Fundraiser"", ""Bereavement / Collation"", ""Meeting / Seminar"", ""Holiday Party"", ""Anniversary"", ""Show / Performance"", ""Other""], ""GridCols"": 2 },
                                { ""Name"": ""otherEventType"", ""Label"": ""Please describe the function"", ""Type"": ""text"", ""Required"": true, ""Placeholder"": ""e.g. Retirement Party"", ""GridCols"": 2, ""Conditional"": { ""Field"": ""eventType"", ""Value"": ""Other"" } },
                                { ""Name"": ""startTime"", ""Label"": ""Start Time"", ""Type"": ""time-split"", ""Required"": true, ""GridCols"": 1 },
                                { ""Name"": ""endTime"", ""Label"": ""End Time"", ""Type"": ""time-split"", ""Required"": true, ""GridCols"": 1 },
                                { ""Name"": ""guestCount"", ""Label"": ""Number of Guests (Max 200)"", ""Type"": ""number"", ""Required"": true, ""Placeholder"": ""Enter number of guests (max 200)"", ""Validation"": { ""Max"": 200 }, ""GridCols"": 1 }
                              ]
                            },
                            {
                              ""Section"": ""Logistics & Services"",
                              ""Fields"": [
                                { ""Name"": ""setupNeeded"", ""Label"": ""Do you require setup time prior to your event start time listed above?"", ""Type"": ""select"", ""Required"": true, ""Options"": [""No"", ""Yes""], ""Description"": ""Please note: The hall will be empty upon your arrival..."" },
                                { ""Name"": ""barService"", ""Label"": ""Bar Service (alcohol & bartender – additional cost)?"", ""Type"": ""select"", ""Required"": true, ""Options"": [""No"", ""Yes""], ""Description"": ""Selecting this option provides a staffed bar with alcoholic beverages..."" },
                                { ""Name"": ""kitchenUse"", ""Label"": ""Kitchen Access Requested?"", ""Type"": ""select"", ""Required"": true, ""Options"": [""No"", ""Yes""], ""Description"": ""GLOUCESTER FRATERNITY CLUB KITCHEN POLICY..."" },
                                { ""Name"": ""catererName"", ""Label"": ""Caterer Name & Insurance Info (Required for cooking)"", ""Type"": ""text"", ""Placeholder"": ""Enter 'Self' or Caterer Name"", ""Conditional"": { ""Field"": ""kitchenUse"", ""Value"": ""Yes"" } },
                                { ""Name"": ""avEquipment"", ""Label"": ""Special Equipment (A/V, Microphone, Projector)?"", ""Type"": ""select"", ""Required"": true, ""Options"": [""No"", ""Yes""], ""Description"": ""Check this if you require use of the club's sound system..."" },
                                { ""Name"": ""details"", ""Label"": ""Additional Notes / Special Requests"", ""Type"": ""textarea"", ""Rows"": 3 }
                              ]
                            },
                            {
                              ""Section"": ""Important Rental Policies"",
                              ""Fields"": [
                                { ""Name"": ""policyFunctionTime"", ""Label"": ""I understand that the rental includes up to 5 hours of function time..."", ""Type"": ""checkbox"", ""Required"": true },
                                { ""Name"": ""policyCommitteeApproval"", ""Label"": ""I understand that all rental requests are subject to Hall Rental Committee approval..."", ""Type"": ""checkbox"", ""Required"": true },
                                { ""Name"": ""policyKitchenRules"", ""Label"": ""I understand that NO CLUB UTENSILS are provided..."", ""Type"": ""checkbox"", ""Required"": true },
                                { ""Name"": ""policySecurityDeposit"", ""Label"": ""I understand that a refundable security deposit is required..."", ""Type"": ""checkbox"", ""Required"": true }
                              ]
                            },
                            {
                              ""Section"": ""Usage Agreement"",
                              ""Fields"": [
                                { ""Name"": ""policyAge"", ""Label"": ""I attest that I am at least 21 years of age."", ""Type"": ""checkbox"", ""Required"": true },
                                { ""Name"": ""policyAlcohol"", ""Label"": ""I understand that NO OUTSIDE ALCOHOL is permitted on the premises..."", ""Type"": ""checkbox"", ""Required"": true },
                                { ""Name"": ""policyDecorations"", ""Label"": ""I agree NOT to use scotch tape, tacks, or nails on any walls..."", ""Type"": ""checkbox"", ""Required"": true },
                                { ""Name"": ""policyCancellation"", ""Label"": ""I understand that cancellations made less than 30 days prior..."", ""Type"": ""checkbox"", ""Required"": true },
                                { ""Name"": ""policyPayment"", ""Label"": ""I agree to pay the Hall Rental Fee in full within 2 business days..."", ""Type"": ""checkbox"", ""Required"": true },
                                { ""Name"": ""policyDamage"", ""Label"": ""I accept full responsibility for any damage to the building..."", ""Type"": ""checkbox"", ""Required"": true }
                              ]
                            }
                          ]
                        }"
                    }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DynamicForms");
        }
    }
}
