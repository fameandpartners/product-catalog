namespace Fame.ImageGeneratorTest
{
    /*public class UnitTest1
    {
        private ComponentType componentType1 = new ComponentType()
        {
            Title = "CT1"
        };

        private ComponentType componentType2 = new ComponentType()
        {
            Title = "CT2"
        };

        private ComponentType componentType3 = new ComponentType()
        {
            Title = "CT3"
        };

        [Fact]
        public void test_createsCombinationsForOneComponent()
        {
            var version = new ProductVersion()
            {
                Options = new List<Option> {
                    CreateOption(
                        componentType: componentType1,
                        componentCode: "C1",
                        renderPosition: "RP1"
                    )
                }
            };

            var generator = new BatchGeneratorService();
            var combinations = generator.GetCombinationsForComponents(version).ToList();

            AssertRenders(new String[] {
                "RP1 -> C1"
            }, combinations);
        }

        [Fact]
        public void test_ignoresComponentesWithoutRenderPosition()
        {
            var version = new ProductVersion()
            {
                Options = new List<Option> {
                    CreateOption(
                        componentType: componentType1,
                        componentCode: "C1",
                        renderPosition: null
                    )
                }
            };

            var generator = new BatchGeneratorService();
            var combinations = generator.GetCombinationsForComponents(version).ToList();

            AssertRenders(new String[] { }, combinations);
        }

        [Fact]
        public void test_createsCombinationsForTwoComponents()
        {
            var version = new ProductVersion()
            {
                Options = new List<Option> {
                    CreateOption(
                        componentType: componentType1,
                        componentCode: "C1",
                        renderPosition: "RP1"
                    ),
                    CreateOption(
                        componentType: componentType1,
                        componentCode: "C2",
                        renderPosition: "RP1"
                    )
                }
            };

            var generator = new BatchGeneratorService();
            var combinations = generator.GetCombinationsForComponents(version).ToList();

            AssertRenders(new String[] {
                "RP1 -> C1",
                "RP1 -> C2"
            }, combinations);
        }


        [Fact]
        public void test_createsCombinationsForComponentsWithRelatedRenderSections()
        {
            var version = new ProductVersion()
            {
                Options = new List<Option> {
                    CreateOption(
                        componentType: componentType1,
                        componentCode: "C1",
                        renderPosition: "RP1",
                        relatedRenderTypes: new ComponentType[] { componentType2 }
                    ),
                    CreateOption(
                        componentType: componentType2,
                        componentCode: "C2",
                        renderPosition: "RP1"
                    )
                }
            };

            var generator = new BatchGeneratorService();
            var combinations = generator.GetCombinationsForComponents(version).ToList();

            AssertRenders(new String[] {
                "RP1 -> C1, C2",
                "RP1 -> C2"
            }, combinations);
        }

        [Fact]
        public void test_createsCombinationsForComponentsWithRelatedRenderSectionsForEachVariation()
        {
            var version = new ProductVersion()
            {
                Options = new List<Option> {
                    CreateOption(
                        componentType: componentType1,
                        componentCode: "C1",
                        renderPosition: "RP1",
                        relatedRenderTypes: new ComponentType[] { componentType2 }
                    ),
                    CreateOption(
                        componentType: componentType2,
                        componentCode: "C2",
                        renderPosition: "RP1"
                    )
                    ,
                    CreateOption(
                        componentType: componentType2,
                        componentCode: "C3",
                        renderPosition: "RP1"
                    )
                }
            };

            var generator = new BatchGeneratorService();
            var combinations = generator.GetCombinationsForComponents(version).ToList();

            AssertRenders(new String[] {
                "RP1 -> C1, C2",
                "RP1 -> C1, C3",
                "RP1 -> C2",
                "RP1 -> C3"
            }, combinations);
        }


        [Fact]
        public void test_createsCombinationsForComponentsWithRelatedRenderSectionsWithMultipleLevels()
        {
            var version = new ProductVersion()
            {
                Options = new List<Option> {
                    CreateOption(
                        componentType: componentType1,
                        componentCode: "C1.1",
                        renderPosition: "RP1",
                        relatedRenderTypes: new ComponentType[] { componentType2, componentType3 }
                    ),
                    CreateOption(
                        componentType: componentType1,
                        componentCode: "C1.2",
                        renderPosition: "RP1",
                        relatedRenderTypes: new ComponentType[] { componentType2, componentType3 }
                    ),
                    CreateOption(
                        componentType: componentType2,
                        componentCode: "C2.1",
                        renderPosition: "RP2"
                    ),
                    CreateOption(
                        componentType: componentType2,
                        componentCode: "C2.2",
                        renderPosition: "RP2"
                    ),
                    CreateOption(
                        componentType: componentType3,
                        componentCode: "C3.1",
                        renderPosition: "RP3",
                        relatedRenderTypes: new ComponentType[] { componentType1 }
                    ),
                    CreateOption(
                        componentType: componentType3,
                        componentCode: "C3.2",
                        renderPosition: "RP3",
                        relatedRenderTypes: new ComponentType[] { componentType1 }
                    )
                }
            };

            var generator = new BatchGeneratorService();
            var combinations = generator.GetCombinationsForComponents(version).ToList();

            AssertRenders(new String[] {
                "RP1 -> C1.1, C2.1, C3.1",
                "RP1 -> C1.1, C2.1, C3.2",
                "RP1 -> C1.1, C2.2, C3.1",
                "RP1 -> C1.1, C2.2, C3.2",
                "RP1 -> C1.2, C2.1, C3.1",
                "RP1 -> C1.2, C2.1, C3.2",
                "RP1 -> C1.2, C2.2, C3.1",
                "RP1 -> C1.2, C2.2, C3.2",

                "RP2 -> C2.1",
                "RP2 -> C2.2",

                "RP3 -> C3.1, C1.1",
                "RP3 -> C3.1, C1.2",
                "RP3 -> C3.2, C1.1",
                "RP3 -> C3.2, C1.2",
            }, combinations);
        }


        [Fact]
        public void test_createsCombinationsForWholeDress()
        {
            var version = new ProductVersion()
            {
                ProductRenderComponents = new List<ProductRenderComponent> {
                    new ProductRenderComponent() {
                        ComponentType = componentType1
                    },
                    new ProductRenderComponent() {
                        ComponentType = componentType2
                    }
                },
                Options = new List<Option> {
                    CreateOption(
                        componentType: componentType1,
                        componentCode: "C1.1",
                        renderPosition: "RP1",
                        relatedRenderTypes: new ComponentType[] { componentType2, componentType3 }
                    ),
                    CreateOption(
                        componentType: componentType1,
                        componentCode: "C1.2",
                        renderPosition: "RP1",
                        relatedRenderTypes: new ComponentType[] { componentType2, componentType3 }
                    ),
                    CreateOption(
                        componentType: componentType2,
                        componentCode: "C2.1",
                        renderPosition: "RP2"
                    ),
                    CreateOption(
                        componentType: componentType2,
                        componentCode: "C2.2",
                        renderPosition: "RP2"
                    ),
                    CreateOption(
                        componentType: componentType3,
                        componentCode: "Ignored",
                        renderPosition: "RP2"
                    )
                }
            };

            var generator = new BatchGeneratorService();
            var renderPositions = new RenderPosition[] { new RenderPosition()
                {
                    RenderPositionId = "RP1"
                }
            };
            var combinations = generator.GetCombinationsForWholeDress(version, renderPositions).ToList();

            AssertRenders(new String[] {
                " -> C1.1, C2.1",
                " -> C1.1, C2.2",
                " -> C1.2, C2.1",
                " -> C1.2, C2.2"
            }, combinations);
        }


        private Option CreateOption(ComponentType componentType, string componentCode, string renderPosition, ComponentType[] relatedRenderTypes = null) {
            return new Option()
            {
                Component = new Component()
                {
                    ComponentType = componentType,
                    ComponentId = componentCode,
                    RenderPosition = renderPosition != null ? new RenderPosition()
                    {
                        RenderPositionId = renderPosition
                    } : null
                },
                OptionRenderComponents = (relatedRenderTypes ?? new ComponentType[0]).Select(x => new OptionRenderComponent()
                {
                    ComponentType = x
                }).ToList()
            };
        }

        private void AssertRenders(string[] expected, ICollection<RenderCombination> actual) {
            var transformedCombinations = actual.Select(renderCombination =>
            {
                var renderPositions = renderCombination.RenderPosition?.RenderPositionId;
                var options = renderCombination.Options.ToArray();

                return $"{renderPositions} -> {String.Join(", ", options)}";
            }).ToArray();

            Assert.Equal(expected, transformedCombinations);
        }
    }*/
}
