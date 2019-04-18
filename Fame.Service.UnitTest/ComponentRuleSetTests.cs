using System;
using System.Collections.Generic;
using Fame.Service.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fame.Service.UnitTest
{
    [TestClass]
    public class ComponentRuleSetTests
    {
        private static IEnumerable<object[]> ComponentRulesData => new[]
        {
            new object[] {"{a1&a2}|{b1&b2}", new List<string> {"a1", "a2"}, true},                                                          // Test bracket priority
            new object[] {"{a1|a2}&{b1|b2}", new List<string> {"a1", "b1"}, true},                                                          // Test bracket priority
            new object[] {"{a1&!a2}", new List<string> {"a1", "a2"}, false},                                                                // Test Not
            new object[] {"{a1}&{!a2|!a3}", new List<string> {"a1", "a3"}, true},                                                           // Test Not
            new object[] {"{a1&a2|a3}", new List<string> {"a3"}, true},                                                                     // Test AND priority
            new object[] {"{1003}&{MM|MN|KN}", new List<string> {"a3", "MM", "1003"}, true},                                                // Test AND priority
            new object[] {"{1003}&{MM|MN|KN}", new List<string> {"a3", "MM", "1001"}, false},                                               // Test AND priority
            new object[] {"{1003}&{MM|MN|KN}", new List<string> {"FPG1001","1013","102","B22","C5","KN","T00","T44","T91","WB2"}, false},   // Test AND priority
            new object[] {"{a1&a2|a3}", new List<string> {"a1", "a2"}, true},                                                               // Test AND priority
            new object[] {"{a1|a2&a3}", new List<string> {"a2", "a3"}, true},                                                               // Test AND priority
            new object[] {"{a1&a2|a3}|{b1&b2&!b3}&{c3}", new List<string> {"b1", "b2", "c3"}, true},                                        // Test EVERYTHING
            new object[] {"{a1&a2|a3}|{b1&b2&!b3}&{c3}", new List<string> {"b1", "b2", "b3", "c3"}, false},                                 // Test EVERYTHING
            new object[] {"{a1&a2|a3}|{b1&b2&!b3}&{c3}", new List<string> {"a1", "a2", "c3"}, true},                                        // Test EVERYTHING
            new object[] {"{a1&a2|a3}|{b1&b2&!b3}&{c3}", new List<string> {"a3", "c3"}, true},                                              // Test EVERYTHING
            new object[] {"{a1&a2|a3}|{b1&b2&!b3}&{c3}", new List<string> {"a3"}, false}                                                    // Test EVERYTHING
        };

        [TestMethod]
        [DynamicData("ComponentRulesData")]
        public void ValidComponentRulesTest(string algorithm, List<string> components, bool expectedMatch)
        {
            var currentRuleSet = new ComponentRuleSet(algorithm, new List<string>());
            var result = currentRuleSet.IsMatch(components);
            Assert.IsTrue(result == expectedMatch);
        }

        private static IEnumerable<object[]> InvalidComponentRulesData => new[]
        {
            new object[] {"a1&a2", new List<string> {"a1", "a2"}},                    // Invalid Missing Brackets
            new object[] {"{a1&a2|{b1&b2}", new List<string> {"a1", "a2"}},           // Invalid Bracket Closure
            new object[] {"", new List<string> {"a1", "a2"}},                         // Invalid Bracket Closure
            new object[] {"a1|a2}&{b1|b2}", new List<string> {"a1", "b1"}},           // Invalid Bracket Opening
            new object[] {"!{a1&a2}", new List<string> {"a1", "a2"}},                 // Invalid Not Usage
            new object[] {"a1&&!a2&!a3", new List<string> {"a1", "a3"}},              // Colocated AND
            new object[] {"a1||a2|a3", new List<string> {"a3"}},                      // Colocated OR
            new object[] {"!!a1|a2|a3", new List<string> {"a3"}},                     // Colocated NOT
            new object[] {"{a1&|a3}|{b1&b2&!b3}", new List<string> {"b1", "b2"}},     // Colocated &|
            new object[] {"{a1|&a3}|{b1&b2&!b3}", new List<string> {"b1", "b2"}},     // Colocated |&
            new object[] {"{a1|}|{b1&b2&!B3}", new List<string> {"b1", "b2"}},        // Missing componentId
            new object[] {"{a1&}|{b1&b2&!B3}", new List<string> {"b1", "b2"}}         // Missing componentId
        };

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid Component Rule.")]
        [DynamicData("InvalidComponentRulesData")]
        public void InvalidComponentRulesTest(string algorithm, List<string> components)
        {
            var currentRuleSet = new ComponentRuleSet(algorithm, new List<string>());
            currentRuleSet.IsMatch(components);
        }
    }
}
