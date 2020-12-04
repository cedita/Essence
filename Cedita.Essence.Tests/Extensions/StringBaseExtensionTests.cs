// Copyright (c) Cedita Ltd. All Rights Reserved.
// Licensed under the Apache License, Version 2.0.

using System;
using Cedita.Essence.Extensions;
using Xunit;

namespace Cedita.Essence.Tests
{
    public class StringBaseExtensionTests
    {
        [Fact]
        public void BasicContainsStringTests()
        {
            Assert.True("This is a string".ContainsAll("This", "is"));
            Assert.False("This is a string".ContainsAll("This", "is", "not"));
            Assert.True("This is a string".ContainsAny("This", "is", "not"));
            Assert.False("This is a string".ContainsAny("not"));
            Assert.True("This is a string".ContainsAll(System.StringComparison.InvariantCultureIgnoreCase, "this"));
            Assert.True("This is a string".ContainsAny(System.StringComparison.InvariantCultureIgnoreCase, "this"));
        }

        [Fact]
        public void BasicContentsStringTests()
        {
            Assert.True("abc".IsAlpha());
            Assert.False("abc123".IsAlpha());
            Assert.True("abc".IsAlphaNumeric());
            Assert.True("abc123".IsAlphaNumeric());
            Assert.False("abc".IsNumeric());
            Assert.False("abc123".IsNumeric());
            Assert.True("123".IsNumeric());
            Assert.True("abba".IsPalindrome());
            Assert.False("bbaa".IsPalindrome());
            Assert.True("blue".IsAnagram("lube"));
            Assert.False("blue".IsAnagram("tube"));
        }

        [Fact]
        public void BasicInternStringTests()
        {
            Assert.NotNull("Test".IsInterned());

            var rand = new Random();
            var nonIntern = string.Empty;
            for (int i = 0; i < rand.Next(2, 10); i++)
            {
                nonIntern += i.ToString();
            }

            Assert.Null(nonIntern.IsInterned());
            Assert.NotNull(nonIntern.Intern());
            Assert.NotNull(nonIntern.IsInterned());
        }

        [Fact]
        public void BasicMaxLengthStringTests()
        {
            Assert.Equal(10, "aaaaaaaaaaaaaaa".MaxLength(10).Length);
            Assert.Equal(10, "aaaaaaaaaaaaaaa".MaxLength(10, "...").Length);
            Assert.Equal("aaaaaaaaaa", "aaaaaaaaaaaaaaa".MaxLength(10));
            Assert.Equal("aaaaaaa...", "aaaaaaaaaaaaaaa".MaxLength(10, "..."));
        }

        [Fact]
        public void BasicNullStringTests()
        {
            Assert.True(default(string).IsNullOrEmpty());
            Assert.True(default(string).IsNullOrWhitespace());
            Assert.True(string.Empty.IsNullOrEmpty());
            Assert.True(string.Empty.IsNullOrWhitespace());
            Assert.False(" ".IsNullOrEmpty());
            Assert.True(" ".IsNullOrWhitespace());
            Assert.False("a".IsNullOrWhitespace());
        }

        [Fact]
        public void BasicOrDefaultTests()
        {
            Assert.Equal("aaaa", string.Empty.OrDefault("aaaa"));
            Assert.Equal("aaaa", default(string).OrDefault("aaaa"));
            Assert.Equal("bbbb", "bbbb".OrDefault("aaaa"));
        }
    }
}
