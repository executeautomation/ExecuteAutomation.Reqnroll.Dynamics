﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by Reqnroll (https://www.reqnroll.net/).
//      Reqnroll Version:2.0.0.0
//      Reqnroll Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace ExecuteAutomation.Reqnroll.Dynamics.Specs.Features
{
    using Reqnroll;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class DynamicRandomTestDataFeature : object, Xunit.IClassFixture<DynamicRandomTestDataFeature.FixtureData>, Xunit.IAsyncLifetime
    {
        
        private global::Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
        private static global::Reqnroll.FeatureInfo featureInfo = new global::Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "DynamicRandomTestData", "Test Random Data from Table", global::Reqnroll.ProgrammingLanguage.CSharp, featureTags);
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "TestRandomTableData.feature"
#line hidden
        
        public DynamicRandomTestDataFeature(DynamicRandomTestDataFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
        }
        
        public static async System.Threading.Tasks.Task FeatureSetupAsync()
        {
        }
        
        public static async System.Threading.Tasks.Task FeatureTearDownAsync()
        {
        }
        
        public async System.Threading.Tasks.Task TestInitializeAsync()
        {
            testRunner = global::Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(featureHint: featureInfo);
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Equals(featureInfo) == false)))
            {
                await testRunner.OnFeatureEndAsync();
            }
            if ((testRunner.FeatureContext == null))
            {
                await testRunner.OnFeatureStartAsync(featureInfo);
            }
        }
        
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            await testRunner.OnScenarioEndAsync();
            global::Reqnroll.TestRunnerManager.ReleaseTestRunner(testRunner);
        }
        
        public void ScenarioInitialize(global::Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public async System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        async System.Threading.Tasks.Task Xunit.IAsyncLifetime.InitializeAsync()
        {
            await this.TestInitializeAsync();
        }
        
        async System.Threading.Tasks.Task Xunit.IAsyncLifetime.DisposeAsync()
        {
            await this.TestTearDownAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Test Random Table Instance implementation with auto.")]
        [Xunit.TraitAttribute("FeatureTitle", "DynamicRandomTestData")]
        [Xunit.TraitAttribute("Description", "Test Random Table Instance implementation with auto.")]
        [Xunit.TraitAttribute("Category", "mytag")]
        public async System.Threading.Tasks.Task TestRandomTableInstanceImplementationWithAuto_()
        {
            string[] tagsOfScenario = new string[] {
                    "mytag"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Test Random Table Instance implementation with auto.", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 6
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
                global::Reqnroll.Table table13 = new global::Reqnroll.Table(new string[] {
                            "Username",
                            "Email",
                            "DateOfBirth",
                            "Phone",
                            "Guid",
                            "Zipcode"});
                table13.AddRow(new string[] {
                            "auto.string",
                            "auto.email",
                            "auto.date",
                            "auto.phone",
                            "auto.guid",
                            "auto.zipcode"});
#line 7
        await testRunner.GivenAsync("users with the following details:", ((string)(null)), table13, "Given ");
#line hidden
#line 10
        await testRunner.ThenAsync("the username should be a valid string", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 11
        await testRunner.AndAsync("the email should be properly formatted", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 12
        await testRunner.AndAsync("the date of birth should be a valid past date", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 13
        await testRunner.AndAsync("the phone number should have a valid format", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 14
        await testRunner.AndAsync("the GUID should be a non-empty unique identifier", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Test Random Table Instance implementation with _")]
        [Xunit.TraitAttribute("FeatureTitle", "DynamicRandomTestData")]
        [Xunit.TraitAttribute("Description", "Test Random Table Instance implementation with _")]
        [Xunit.TraitAttribute("Category", "mytag")]
        public async System.Threading.Tasks.Task TestRandomTableInstanceImplementationWith_()
        {
            string[] tagsOfScenario = new string[] {
                    "mytag"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Test Random Table Instance implementation with _", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 17
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
                global::Reqnroll.Table table14 = new global::Reqnroll.Table(new string[] {
                            "Username",
                            "Email",
                            "DateOfBirth",
                            "Phone",
                            "Guid",
                            "Zipcode"});
                table14.AddRow(new string[] {
                            "_",
                            "_",
                            "_",
                            "_",
                            "_",
                            "_"});
#line 18
        await testRunner.GivenAsync("users with the following details:", ((string)(null)), table14, "Given ");
#line hidden
#line 21
        await testRunner.ThenAsync("the user set should contain 1 user", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 22
        await testRunner.AndAsync("all users should have valid usernames", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 23
        await testRunner.AndAsync("all users should have valid email addresses", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 24
        await testRunner.AndAsync("all users should have valid dates of birth", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 25
        await testRunner.AndAsync("all users should have valid phone numbers", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 26
        await testRunner.AndAsync("all users should have valid GUIDs", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 27
        await testRunner.AndAsync("all users should have valid zipcodes", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 28
        await testRunner.AndAsync("all users should have all fields populated with valid data", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Test Random Table Instance implementation with multiple rows using _")]
        [Xunit.TraitAttribute("FeatureTitle", "DynamicRandomTestData")]
        [Xunit.TraitAttribute("Description", "Test Random Table Instance implementation with multiple rows using _")]
        [Xunit.TraitAttribute("Category", "mytag")]
        public async System.Threading.Tasks.Task TestRandomTableInstanceImplementationWithMultipleRowsUsing_()
        {
            string[] tagsOfScenario = new string[] {
                    "mytag"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Test Random Table Instance implementation with multiple rows using _", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 31
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
                global::Reqnroll.Table table15 = new global::Reqnroll.Table(new string[] {
                            "Username",
                            "Email",
                            "DateOfBirth",
                            "Phone",
                            "Guid",
                            "Zipcode"});
                table15.AddRow(new string[] {
                            "_",
                            "_",
                            "_",
                            "_",
                            "_",
                            "_"});
                table15.AddRow(new string[] {
                            "_",
                            "_",
                            "_",
                            "_",
                            "_",
                            "_"});
                table15.AddRow(new string[] {
                            "_",
                            "_",
                            "_",
                            "_",
                            "_",
                            "_"});
#line 32
        await testRunner.GivenAsync("users with the following details:", ((string)(null)), table15, "Given ");
#line hidden
#line 37
        await testRunner.ThenAsync("the user set should contain 3 users", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 38
        await testRunner.AndAsync("all users should have valid usernames", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 39
        await testRunner.AndAsync("all users should have valid email addresses", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 40
        await testRunner.AndAsync("all users should have valid dates of birth", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 41
        await testRunner.AndAsync("all users should have valid phone numbers", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 42
        await testRunner.AndAsync("all users should have valid GUIDs", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 43
        await testRunner.AndAsync("all users should have valid zipcodes", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 44
        await testRunner.AndAsync("all users should have all fields populated with valid data", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Test Random Table Instance with comprehensive data types")]
        [Xunit.TraitAttribute("FeatureTitle", "DynamicRandomTestData")]
        [Xunit.TraitAttribute("Description", "Test Random Table Instance with comprehensive data types")]
        [Xunit.TraitAttribute("Category", "mytag")]
        public async System.Threading.Tasks.Task TestRandomTableInstanceWithComprehensiveDataTypes()
        {
            string[] tagsOfScenario = new string[] {
                    "mytag"};
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Test Random Table Instance with comprehensive data types", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 47
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
                global::Reqnroll.Table table16 = new global::Reqnroll.Table(new string[] {
                            "StringValue",
                            "IntValue",
                            "BoolValue",
                            "DecimalValue",
                            "DateTimeValue",
                            "GuidValue",
                            "UriValue",
                            "TimeSpanValue",
                            "EmailValue",
                            "PhoneValue",
                            "CreditCardValue",
                            "CountryValue",
                            "CityValue",
                            "FirstNameValue",
                            "LastNameValue",
                            "StringListValue",
                            "IntListValue"});
                table16.AddRow(new string[] {
                            "auto.string",
                            "auto.int",
                            "auto.bool",
                            "auto.decimal",
                            "auto.datetime",
                            "auto.guid",
                            "auto.uri",
                            "auto.timespan",
                            "auto.email",
                            "auto.phone",
                            "auto.creditcard",
                            "auto.country",
                            "auto.city",
                            "auto.firstname",
                            "auto.lastname",
                            "auto.stringlist",
                            "auto.intlist"});
#line 48
        await testRunner.GivenAsync("users with the following details:", ((string)(null)), table16, "Given ");
#line hidden
#line 51
        await testRunner.ThenAsync("all auto-generated fields should have valid values", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : object, Xunit.IAsyncLifetime
        {
            
            async System.Threading.Tasks.Task Xunit.IAsyncLifetime.InitializeAsync()
            {
                await DynamicRandomTestDataFeature.FeatureSetupAsync();
            }
            
            async System.Threading.Tasks.Task Xunit.IAsyncLifetime.DisposeAsync()
            {
                await DynamicRandomTestDataFeature.FeatureTearDownAsync();
            }
        }
    }
}
#pragma warning restore
#endregion
