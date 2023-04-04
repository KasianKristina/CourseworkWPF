using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        /// <summary>
        /// Тест для проверки метода NearbyMove. Все позиции достижимы.
        /// </summary>
        public void moveNearbyPositionTest1()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            bool check = player1.queen.NearbyMove(player2.king.Offset.Row, player2.king.Offset.Column, 1, player1.history);
            Assert.AreEqual(true, check);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода NearbyMove. Одна возможная позиция.
        /// </summary>
        public void moveNearbyPositionTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player2.king.MoveBlock(0, 4);

            bool check = player1.queen.NearbyMove(player2.king.Offset.Row, player2.king.Offset.Column, 1, player1.history);
            Assert.AreEqual(true, check);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода NearbyMove. Нет возможных позиций.
        /// </summary>
        public void moveNearbyPositionTest3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player2.king.MoveBlock(1, 4);
            GameField[0, 2] = -5; // стена

            bool check = player1.queen.NearbyMove(player2.king.Offset.Row, player2.king.Offset.Column, 1, player1.history);
            Assert.AreEqual(false, check);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getObstaclesPosition. Есть позиции блокировки.
        /// </summary>
        public void getObstaclesPositionTest()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveBlock(5, 5);
            GameField[6, 7] = -5; // стена
            GameField[6, 5] = -5; // стена

            List<Position> listObstacles = player2.queen.GetObstaclesPosition(player1.king, player2.Color);
            List<Position> expectedList = new List<Position>() {
                            new Position(6, 6),
                            new Position(6, 4),
                            new Position(6, 3),
                            new Position(6, 2),
                            new Position(6, 1),
                            new Position(6, 0)};
            CollectionAssert.AreEqual(expectedList, listObstacles);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getObstaclesPosition. Есть позиции блокировки. Стена преграждает путь.
        /// </summary>
        public void getObstaclesPositionTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveBlock(5, 5);
            GameField[6, 7] = -5; // стена
            GameField[6, 5] = -5; // стена
            GameField[6, 2] = -5; // стена

            List<Position> listObstacles = player2.queen.GetObstaclesPosition(player1.king, player2.Color);
            List<Position> expectedList = new List<Position>() {
                            new Position(6, 6),
                            new Position(6, 4),
                            new Position(6, 3) };
            CollectionAssert.AreEqual(expectedList, listObstacles);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getObstaclesPosition. Нет позиций блокировки.
        /// </summary>
        public void getObstaclesPositionTest3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveBlock(5, 5);
            GameField[6, 7] = -5; // стена
            GameField[6, 6] = -5;
            GameField[6, 4] = -5;
            GameField[6, 3] = -5;
            GameField[6, 2] = -5;
            GameField[6, 1] = -5;
            GameField[6, 0] = -5;

            List<Position> listObstacles = player2.queen.GetObstaclesPosition(player1.king, player2.Color);
            List<Position> expectedList = new List<Position>() { };
            CollectionAssert.AreEqual(expectedList, listObstacles);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода ObstacleMove. Нельзя дважды блокировать короля соперника.
        /// </summary>
        public void getObstacleMoveTest1()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;
            GameField[1, 1] = -5;

            player2.king.MoveBlock(2, 4);
            player2.history.Add(motion, (player2.king.Id, new Position(2, 4)));
            player1.queen.MoveBlock(1, 2);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 2)));
            motion++;

            player2.king.MoveBlock(2, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(2, 5)));
            player1.queen.MoveBlock(0, 2);
            player1.history.Add(motion, (player1.queen.Id, new Position(0, 2)));
            motion++;

            player2.king.MoveBlock(2, 4);
            player2.history.Add(motion, (player2.king.Id, new Position(2, 4)));

            bool move = player1.queen.ObstacleMove(player2.king, player1.Color, 0, player1.history, motion);
            Assert.AreEqual(false, move);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода ObstacleMove. Есть позиции блокировки.
        /// </summary>
        public void getObstacleMoveTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            player1.king.MoveBlock(1, 5);
            player1.history.Add(motion, (player1.king.Id, new Position(1, 5)));
            player2.king.MoveBlock(6, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(6, 5)));
            motion++;

            player1.king.MoveBlock(2, 6);
            player1.history.Add(motion, (player1.king.Id, new Position(2, 6)));
            player2.king.MoveBlock(2, 4);
            player2.history.Add(motion, (player2.king.Id, new Position(2, 4)));
            motion++;

            bool move = player1.queen.ObstacleMove(player2.king, player1.Color, 2, player1.history, motion);
            Assert.AreEqual(true, move);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getPozFerz. Ферзь уже блокирует короля.
        /// </summary>
        public void getPozFerzTest1()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.queen.MoveBlock(4, 1);
            player2.king.MoveBlock(5, 3);

            bool check = player1.queen.GetPosFerz(player2.king.Offset.Row, player2.king.Offset.Column, player1.Color);
            Assert.AreEqual(true, check);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getPozFerz. Ферзь не блокирует короля.
        /// </summary>
        public void getPozFerzTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.queen.MoveBlock(3, 1);
            player2.king.MoveBlock(5, 3);

            bool check = player1.queen.GetPosFerz(player2.king.Offset.Row, player2.king.Offset.Column, player1.Color);
            Assert.AreEqual(false, check);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getPozFerz. Черный ферзь блокирует короля.
        /// </summary>
        public void getPozFerzTest3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveBlock(4, 4);
            player2.queen.MoveBlock(5, 6);

            bool check = player2.queen.GetPosFerz(player1.king.Offset.Row, player1.king.Offset.Column, player2.Color);
            Assert.AreEqual(true, check);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getPozFerz. Черный ферзь не блокирует короля.
        /// </summary>
        public void getPozFerzTest4()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveBlock(4, 4);
            player2.queen.MoveBlock(6, 3);

            bool check = player2.queen.GetPosFerz(player1.king.Offset.Row, player1.king.Offset.Column, player2.Color);
            Assert.AreEqual(false, check);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода CheckPreviousPosition
        /// </summary>
        public void CheckPreviousPositionTest()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            player1.queen.MoveBlock(1, 2);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 2)));
            motion++;

            player1.queen.MoveBlock(2, 2);
            player1.history.Add(motion, (player1.queen.Id, new Position(2, 2)));
            motion++;

            player1.queen.MoveBlock(0, 3);
            player1.history.Add(motion, (player1.queen.Id, new Position(0, 3)));
            motion++;

            int row = player1.queen.CheckPreviousPosition(player1.history);
            Assert.AreEqual(2, row);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода CheckPreviousPosition
        /// </summary>
        public void CheckPreviousPositionTest1()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            player1.queen.MoveBlock(1, 2);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 2)));
            motion++;

            int row = player1.queen.CheckPreviousPosition(player1.history);
            Assert.AreEqual(-10, row);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода CheckPreviousPosition
        /// </summary>
        public void CheckPreviousPositionTest3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            player1.queen.MoveBlock(1, 2);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 2)));
            motion++;

            player1.queen.MoveBlock(2, 2);
            player1.history.Add(motion, (player1.queen.Id, new Position(2, 2)));
            motion++;

            int row = player1.queen.CheckPreviousPosition(player1.history);
            Assert.AreEqual(1, row);
        }

        [TestMethod, TestCategory("BlocksPosition")]
        /// <summary>
        /// Тест для проверки метода getBlocksPositionsTest (ферзь > короля)
        /// </summary>
        public void getBlocksPositionsTest()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            player1.king.MoveBlock(1, 2);
            player1.history.Add(motion, (player1.king.Id, new Position(1, 2)));
            motion++;

            player2.queen.MoveBlock(2, 5);
            player2.history.Add(motion, (player2.queen.Id, new Position(2, 5)));
            motion++;

            List<Position> listObstacles = player1.queen.GetBlocksPositions(player1.king.Offset.Column, player1.Сompetitor.queen.Offset.Row, player1.Сompetitor.queen.Offset.Column);
            List<Position> expectedList = new List<Position>() {
                            new Position(2, 3),
                            new Position(2, 4) };
            CollectionAssert.AreEqual(expectedList, listObstacles);
        }

        [TestMethod, TestCategory("BlocksPosition")]
        /// <summary>
        /// Тест для проверки метода getBlocksPositionsTest (ферзь < короля)
        /// </summary>
        public void getBlocksPositionsTest1()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveBlock(1, 6);
            player2.queen.MoveBlock(2, 1);

            List<Position> listObstacles = player1.queen.GetBlocksPositions(player1.king.Offset.Column, player1.Сompetitor.queen.Offset.Row, player1.Сompetitor.queen.Offset.Column);
            List<Position> expectedList = new List<Position>() {
                            new Position(2, 5),
                            new Position(2, 4),
                            new Position(2, 3),
                            new Position(2, 2)};
            CollectionAssert.AreEqual(expectedList, listObstacles);
        }

        [TestMethod, TestCategory("Equals")]
        /// <summary>
        /// Тест для проверки равенства
        /// </summary>
        public void getEqualsPosTest()
        {
            Position pos = new Position(2, 3);
            Position pos2 = new Position(2, 3);
            Assert.AreEqual(true, pos.Equals(pos2));
            Position pos3 = new Position(2, 5);
            Position pos4 = new Position(2, 3);
            Assert.AreEqual(false, pos3.Equals(pos4));
        }

        [TestMethod, TestCategory("LeaveSquare")]
        /// <summary>
        /// Тест для проверки LeaveSquareCheck
        /// </summary>
        public void LeaveSquareCheckTest()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            Assert.AreEqual(true, player1.king.LeaveSquareCheck(5, 1));
        }

        [TestMethod, TestCategory("LeaveSquare")]
        /// <summary>
        /// Тест для проверки LeaveSquareCheck
        /// </summary>
        public void LeaveSquareCheckTest1()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
           
            player1.king.MoveBlock(0, 5);
            Assert.AreEqual(true, player1.king.LeaveSquareCheck(6, 2));
        }

        [TestMethod, TestCategory("LeaveSquare")]
        /// <summary>
        /// Тест для проверки LeaveSquareCheck
        /// </summary>
        public void LeaveSquareCheckTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveBlock(0, 6);
            Assert.AreEqual(false, player1.king.LeaveSquareCheck(3, 5));
        }
    }
}
