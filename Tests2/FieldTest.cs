using System;
using System.Collections.Generic;
using FluentAssertions;
using UglyTetris.GameLogic;
using Xunit;

namespace Tests2
{
    public class FieldTest
    {
        private static Tile T() => new Tile("Brown");

        private static List<Field> Fields = new List<Field>()
        {
            new Field(new Tile[,]
            {
                // 0 -> Y
                // | 
                // v X 
                {T(), T(), T(), T(), T(), T(), T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {null, null, null, null, null, null, T(), T(),},
                {T(), T(), T(), T(), T(), T(), T(), T(),},
            }),
            new Field(new Tile[,]
                {
                    // 0 -> Y
                    // | 
                    // v X 
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, null, T(), T(), T(),},
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                }
            ),
            new Field(new Tile[,]
                {
                    // 0 -> Y
                    // | 
                    // v X 
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), null, T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), null, T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, T(), T(), T(), T(),},
                    {null, null, null, null, null, T(), T(), T(),},
                    {T(), T(), T(), T(), T(), T(), T(), T(),},
                }
            )
        };


        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 0)]
        public void RemoveLines(int fieldIndex, int lineCount)
        {
            var field = Fields[fieldIndex];
            field.RemoveFullLines().Should().Be(lineCount);
        }

        [Theory]
        [InlineData(0)]
        public void CheckDownFigureSuccess(int fieldIndex)
        {
            var field = Fields[fieldIndex];
            var f = new FigureFactory().CreateStandardFigure(FigureType.I);

            field.IsPossibleToPlaceFigure(f, 3, 5).Should().Be(false);
        }
        
        [Theory]
        [InlineData(0)]
        public void CheckDownFigureFail(int fieldIndex)
        {
            var field = Fields[fieldIndex];
            var f = new FigureFactory().CreateStandardFigure(FigureType.I);

            field.IsPossibleToPlaceFigure(f, 3, 0).Should().Be(true);
        }

        [Theory]
        [InlineData(0)]
        public void CheckSetTileToMissField(int fieldIndex)
        {
            var field = Fields[fieldIndex];
            var tile = new Tile("red");
            var check = false;
            try
            {
                field.SetTile(500, 500, tile);
            } catch (Exception e)
            {
                check = true;
            }

            check.Should().Be(true);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void CheckFillTileSuccess(int fieldIndex)
        {
            var field = Fields[fieldIndex];
            field.IsEmpty(1, 6).Should().Be(false);
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void CheckFillTileFail(int fieldIndex)
        {
            var field = Fields[fieldIndex];
            field.IsEmpty(2, 0).Should().Be(true);
        }
    }
    
    
}