﻿using System;
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
        /// Тест для проверки метода getObstaclesPosition. Есть позиции блокировки.
        /// </summary>
        public void getObstaclesPositionTest()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveFigure(5, 5);
            GameField[6, 7] = -5; // стена
            GameField[6, 5] = -5; // стена

            List<Position> listObstacles = player2.queen.GetObstaclesPosition(player1.king);
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

            player1.king.MoveFigure(5, 5);
            GameField[6, 7] = -5; // стена
            GameField[6, 5] = -5; // стена
            GameField[6, 2] = -5; // стена

            List<Position> listObstacles = player2.queen.GetObstaclesPosition(player1.king);
            List<Position> expectedList = new List<Position>() {
                            new Position(6, 6),
                            new Position(6, 4),
                            new Position(6, 3) };
            CollectionAssert.AreEqual(expectedList, listObstacles);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getObstaclesPosition. Есть позиции блокировки. Стена преграждает путь.
        /// </summary>
        public void getObstaclesPositionTest4()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveFigure(2, 4);

            List<Position> listObstacles = player2.queen.GetObstaclesPosition(player1.king);
            List<Position> expectedList = new List<Position>() {
                            new Position(3, 5),
                            new Position(3, 3),
                            new Position(3, 6),
                            new Position(3, 2),
                            new Position(3, 7),
                            new Position(3, 1),
                            new Position(3, 0)};
            CollectionAssert.AreEqual(expectedList, listObstacles);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getObstaclesPosition. Есть позиции блокировки. Стена преграждает путь.
        /// </summary>
        public void getObstaclesPositionTest5()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveFigure(1, 2);

            List<Position> listObstacles = player2.queen.GetObstaclesPosition(player1.king);
            List<Position> expectedList = new List<Position>() {
                            new Position(2, 3),
                            new Position(2, 1),
                            new Position(2, 4),
                            new Position(2, 0),
                            new Position(2, 5),
                            new Position(2, 6),
                            new Position(2, 7)};
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

            player1.king.MoveFigure(5, 5);
            GameField[6, 7] = -5; // стена
            GameField[6, 6] = -5;
            GameField[6, 4] = -5;
            GameField[6, 3] = -5;
            GameField[6, 2] = -5;
            GameField[6, 1] = -5;
            GameField[6, 0] = -5;

            List<Position> listObstacles = player2.queen.GetObstaclesPosition(player1.king);
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

            player2.king.MoveFigure(2, 4);
            player2.history.Add(motion, (player2.king.Id, new Position(2, 4)));
            player1.queen.MoveFigure(1, 2);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 2)));
            motion++;

            player2.king.MoveFigure(2, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(2, 5)));
            player1.queen.MoveFigure(0, 2);
            player1.history.Add(motion, (player1.queen.Id, new Position(0, 2)));
            motion++;

            player2.king.MoveFigure(2, 4);
            player2.history.Add(motion, (player2.king.Id, new Position(2, 4)));

            bool move = player1.queen.ObstacleMove(player2.king, 0, player1.history, motion);
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

            player1.king.MoveFigure(1, 5);
            player1.history.Add(motion, (player1.king.Id, new Position(1, 5)));
            player2.king.MoveFigure(6, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(6, 5)));
            motion++;

            player1.king.MoveFigure(2, 6);
            player1.history.Add(motion, (player1.king.Id, new Position(2, 6)));
            player2.king.MoveFigure(2, 4);
            player2.history.Add(motion, (player2.king.Id, new Position(2, 4)));
            motion++;

            bool move = player1.queen.ObstacleMove(player2.king, 2, player1.history, motion);
            Assert.AreEqual(true, move);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода ChooseNonPregradaPosition. Есть удачная позиция для хода короля.
        /// </summary>
        public void ChooseNonPregradaPosition()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            player1.king.MoveFigure(5, 4);
            player1.history.Add(motion, (player1.king.Id, new Position(5, 4)));
            player2.king.MoveFigure(0, 0);
            player2.history.Add(motion, (player2.king.Id, new Position(0, 0)));
            motion++;

            player1.queen.MoveFigure(7, 1);
            player1.history.Add(motion, (player1.queen.Id, new Position(7, 1)));
            player2.queen.MoveFigure(2, 6);
            player2.history.Add(motion, (player2.queen.Id, new Position(2, 6)));
            motion++;

            List<Position> listCheckPositions = player1.king.ChooseNonPregradaPosition(player1.Сompetitor.queen, player1.Сompetitor.king, motion, player1.queen);
            List<Position> list = new List<Position>() { new Position(6, 5) };
            CollectionAssert.AreEqual(list, listCheckPositions);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода ChooseNonPregradaPosition. Есть удачная позиция для хода короля.
        /// </summary>
        public void ChooseNonPregradaPositionNumber2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            player1.king.MoveFigure(7, 7);
            player1.history.Add(motion, (player1.king.Id, new Position(7, 7)));
            player2.king.MoveFigure(3, 4);
            player2.history.Add(motion, (player2.king.Id, new Position(3, 4)));
            motion++;

            player1.queen.MoveFigure(0, 6);
            player1.history.Add(motion, (player1.queen.Id, new Position(0, 6)));
            player2.queen.MoveFigure(5, 7);
            player2.history.Add(motion, (player2.queen.Id, new Position(5, 7)));
            motion++;

            GameField[1, 7] = -5; // стена

            List<Position> listCheckPositions = player2.king.ChooseNonPregradaPosition(player2.Сompetitor.queen, player2.Сompetitor.king, motion, player2.queen);
            List<Position> list = new List<Position>() { new Position(2, 5) };
            CollectionAssert.AreEqual(list, listCheckPositions);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода ChooseNonPregradaPosition. Есть стены и один удачный ход.
        /// </summary>
        public void ChooseNonPregradaPositionNumber3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            player1.king.MoveFigure(7, 7);
            player1.history.Add(motion, (player1.king.Id, new Position(7, 7)));
            player2.king.MoveFigure(3, 6);
            player2.history.Add(motion, (player2.king.Id, new Position(3, 6)));
            motion++;

            player1.queen.MoveFigure(0, 6);
            player1.history.Add(motion, (player1.queen.Id, new Position(0, 6)));
            player2.queen.MoveFigure(5, 6);
            player2.history.Add(motion, (player2.queen.Id, new Position(5, 6)));
            motion++;

            GameField[2, 6] = -5; // стена
            GameField[1, 7] = -5; // стена

            List<Position> listCheckPositions = player2.king.ChooseNonPregradaPosition(player2.Сompetitor.queen, player2.Сompetitor.king, motion, player2.queen);
            List<Position> list = new List<Position>() { new Position(2, 5) };
            CollectionAssert.AreEqual(list, listCheckPositions);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода ChooseNonPregradaPosition. Нет удачных ходов.
        /// </summary>
        public void ChooseNonPregradaPositionNumber4()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            player1.king.MoveFigure(7, 7);
            player1.history.Add(motion, (player1.king.Id, new Position(7, 7)));
            player2.king.MoveFigure(3, 6);
            player2.history.Add(motion, (player2.king.Id, new Position(3, 6)));
            motion++;

            player1.queen.MoveFigure(0, 6);
            player1.history.Add(motion, (player1.queen.Id, new Position(0, 6)));
            player2.queen.MoveFigure(5, 6);
            player2.history.Add(motion, (player2.queen.Id, new Position(5, 6)));
            motion++;

            GameField[2, 6] = -5; // стена
            GameField[1, 7] = -5; // стена
            GameField[2, 5] = -5; // стена

            List<Position> listCheckPositions = player2.king.ChooseNonPregradaPosition(player2.Сompetitor.queen, player2.Сompetitor.king, motion, player2.queen);
            List<Position> list = new List<Position>() { };
            CollectionAssert.AreEqual(list, listCheckPositions);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода ChooseNonPregradaPosition. Несколько  удачных ходов.
        /// </summary>
        public void ChooseNonPregradaPositionNumber5()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            player1.king.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.king.Id, new Position(1, 3)));
            player2.king.MoveFigure(6, 4);
            player2.history.Add(motion, (player2.king.Id, new Position(6, 4)));
            motion++;

            player1.queen.MoveFigure(4, 7);
            player1.history.Add(motion, (player1.queen.Id, new Position(4, 7)));
            player2.queen.MoveFigure(0, 0);
            player2.history.Add(motion, (player2.queen.Id, new Position(0, 0)));
            motion++;

            GameField[2, 3] = -5; // стена
            GameField[2, 4] = -5; // стена
            GameField[3, 3] = -5; // стена
            GameField[4, 6] = -5; // стена

            List<Position> listCheckPositions = player2.king.ChooseNonPregradaPosition(player2.Сompetitor.queen, player2.Сompetitor.king, motion, player2.queen);
            List<Position> list = new List<Position>() { new Position(5, 3), new Position(5, 4), new Position(5, 5) };
            CollectionAssert.AreEqual(list, listCheckPositions);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода SameWayMoveGetPath.
        /// </summary>
        public void SameWayMoveGetPathTest1()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            player1.king.MoveFigure(7, 2);
            player1.history.Add(motion, (player1.king.Id, new Position(7, 2)));
            player2.king.MoveFigure(2, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(2, 5)));
            motion++;

            player1.queen.MoveFigure(5, 1);
            player1.history.Add(motion, (player1.queen.Id, new Position(5, 1)));
            player2.queen.MoveFigure(4, 7);
            player2.history.Add(motion, (player2.queen.Id, new Position(4, 7)));
            motion++;

            GameField[1, 5] = -5; // стена

            List<Position> way = player2.king.SameWayMoveGetPath(new Position(0, 4));
            List<Position> list = new List<Position>() { new Position(1, 4), new Position(0, 4) };
            CollectionAssert.AreEqual(list, way);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода SameWayMoveGetPath.
        /// </summary>
        public void SameWayMoveGetPathTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            player2.king.MoveFigure(3, 4);
            player2.history.Add(motion, (player2.king.Id, new Position(2, 5)));
            motion++;

            GameField[1, 5] = -5; // стена
            GameField[2, 3] = -5;
            GameField[2, 4] = -5;

            List<Position> way = player2.king.SameWayMoveGetPath(new Position(0, 4));
            List<Position> list = new List<Position>() { new Position(2, 5), new Position(1, 4), new Position(0, 4) };
            CollectionAssert.AreEqual(list, way);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода SameWayMoveGetPath.
        /// </summary>
        public void SameWayMoveGetPathTest3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            player1.king.MoveFigure(4, 1);
            player1.history.Add(motion, (player1.king.Id, new Position(4, 1)));
            motion++;

            List<Position> way = player1.king.SameWayMoveGetPath(new Position(7, 4));
            List<Position> list = new List<Position>() { new Position(5, 2), new Position(6, 3), new Position(7, 4) };
            CollectionAssert.AreEqual(list, way);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getResult.
        /// </summary>
        public void getResultTest1()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;
            player1.king.MoveFigure(1, 4);
            player1.queen.MoveFigure(0, 3);
            player2.king.MoveFigure(7, 4);
            player2.queen.MoveFigure(7, 3);

            GameField[1, 1] = -5; // стена
            GameField[2, 4] = -5;
            GameField[2, 6] = -5;
            GameField[3, 0] = -5;
            GameField[3, 1] = -5;
            GameField[4, 0] = -5;
            GameField[4, 5] = -5;
            GameField[5, 5] = -5;
            GameField[6, 1] = -5;
            GameField[6, 6] = -5;
            GameField[6, 7] = -5;
            GameField[4, 4] = -5;
            GameField[3, 3] = -5;
            GameField[2, 3] = -5;

            int result = DynamicField.GetResultMinMax(player1);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getResult. (-6, -6)
        /// </summary>
        public void getResultTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;
            player1.king.MoveFigure(4, 1);
            player1.queen.MoveFigure(3, 7);
            player2.king.MoveFigure(4, 3);
            player2.queen.MoveFigure(5, 3);

            GameField[1, 1] = -5; // стена
            GameField[2, 4] = -5;
            GameField[2, 6] = -5;
            GameField[3, 0] = -5;
            GameField[3, 1] = -5;
            GameField[4, 0] = -5;
            GameField[4, 5] = -5;
            GameField[5, 5] = -5;
            GameField[6, 1] = -5;
            GameField[6, 6] = -5;
            GameField[6, 7] = -5;
            GameField[4, 4] = -5;
            GameField[3, 3] = -5;
            GameField[2, 3] = -5;

            int result = DynamicField.GetResultMinMax(player1);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getResult. (-100, 100) === -200
        /// </summary>
        public void getResultTest3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;
            player1.king.MoveFigure(5, 6);
            player1.queen.MoveFigure(2, 5);
            player2.king.MoveFigure(3, 2);
            player2.queen.MoveFigure(0, 7);

            GameField[1, 7] = -5; // стена
            GameField[5, 1] = -5;
            GameField[5, 2] = -5;

            int result = DynamicField.GetResultMinMax(player1);

            Assert.AreEqual(-200, result);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getResult. (4, 100) === -96
        /// </summary>
        public void getResultTest4()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;
            player1.king.MoveFigure(5, 6);
            player1.queen.MoveFigure(2, 5);
            player2.king.MoveFigure(3, 2);
            player2.queen.MoveFigure(0, 7);

            GameField[1, 7] = -5; // стена
            GameField[5, 1] = -5;
            GameField[5, 2] = -5;
            GameField[6, 4] = -5; // стена
            GameField[6, 5] = -5;
            GameField[6, 6] = -5;

            int result = DynamicField.GetResultMinMax(player1);

            Assert.AreEqual(-96, result);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getResult. (4, 6) === -2
        /// </summary>
        public void getResultTest5()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;
            player1.king.MoveFigure(5, 6);
            player1.queen.MoveFigure(2, 5);
            player2.king.MoveFigure(3, 2);
            player2.queen.MoveFigure(0, 7);

            GameField[1, 7] = -5; // стена
            GameField[5, 1] = -5;
            GameField[5, 2] = -5;
            GameField[6, 4] = -5; // стена
            GameField[6, 5] = -5;
            GameField[6, 6] = -5;
            GameField[2, 1] = -5;

            int result = DynamicField.GetResultMinMax(player1);

            Assert.AreEqual(-2, result);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getResult. (-200, 100) === -300
        /// </summary>
        public void getResultTest6()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;
            player1.king.MoveFigure(6, 4);
            player1.queen.MoveFigure(1, 0);
            player2.king.MoveFigure(2, 6);
            player2.queen.MoveFigure(5, 7);

            GameField[0, 2] = -5; // стена
            GameField[3, 0] = -5;
            GameField[3, 1] = -5;
            GameField[3, 2] = -5;
            GameField[4, 0] = -5;
            GameField[4, 1] = -5;
            GameField[4, 2] = -5;
            GameField[5, 0] = -5;
            GameField[6, 0] = -5;
            GameField[7, 0] = -5;
            GameField[4, 6] = -5;
            GameField[6, 5] = -5;
            GameField[6, 6] = -5;
            GameField[6, 7] = -5;

            int result = DynamicField.GetResultMinMax(player1);

            Assert.AreEqual(-300, result);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода minMax. Result: (-300, 100) === -400
        /// </summary>
        public void getMinMaxTest1()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;
            player1.king.MoveFigure(6, 4);
            player1.queen.MoveFigure(1, 0);
            player2.king.MoveFigure(2, 6);
            player2.queen.MoveFigure(5, 7);

            GameField[0, 2] = -5; // стена
            GameField[3, 0] = -5;
            GameField[3, 1] = -5;
            GameField[3, 2] = -5;
            GameField[4, 0] = -5;
            GameField[4, 1] = -5;
            GameField[4, 2] = -5;
            GameField[5, 0] = -5;
            GameField[6, 0] = -5;
            GameField[7, 0] = -5;
            GameField[4, 6] = -5;
            GameField[6, 5] = -5;
            GameField[6, 6] = -5;
            GameField[6, 7] = -5;

            //int result = DynamicField.minMax(player1, 1, player1.king.Offset, player1.queen.Offset, player2.king.Offset, player2.queen.Offset, int.MinValue, int.MaxValue, 1, player2.history, motion, player2.GameField, player2.motionColor);

            //Assert.AreEqual(-400, result);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода minMax
        /// </summary>
        public void getMinMaxTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;
            player1.king.MoveFigure(6, 4);
            player1.queen.MoveFigure(1, 1);
            player2.king.MoveFigure(2, 6);
            player2.queen.MoveFigure(5, 7);

            GameField[0, 2] = -5; // стена
            GameField[3, 0] = -5;
            GameField[3, 1] = -5;
            GameField[3, 2] = -5;
            GameField[4, 0] = -5;
            GameField[4, 1] = -5;
            GameField[4, 2] = -5;
            GameField[5, 0] = -5;
            GameField[6, 0] = -5;
            GameField[7, 0] = -5;
            GameField[4, 6] = -5;
            GameField[6, 5] = -5;
            GameField[6, 6] = -5;
            GameField[6, 7] = -5;

            //int result = DynamicField.minMax(player2, 0, player1.king.Offset, player1.queen.Offset, player2.king.Offset, player2.queen.Offset, int.MinValue, int.MaxValue, 0, player2.history, motion, player2.GameField, player2.motionColor);

            //Assert.AreEqual(-300, result);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода minMax. Позиция для хода к1 = (6, 5).  -204
        /// </summary>
        public void getMinMaxTest3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;
            player1.king.MoveFigure(5, 5);
            player1.queen.MoveFigure(7, 0);
            player2.king.MoveFigure(4, 1);
            player2.queen.MoveFigure(0, 0);

            GameField[1, 1] = -5; // стена
            GameField[5, 0] = -5;
            GameField[6, 0] = -5;
            GameField[6, 1] = -5;

            //int result = DynamicField.minMax(player1, 1, player1.king.Offset, player1.queen.Offset, player2.king.Offset, player2.queen.Offset, int.MinValue, int.MaxValue, 1, player1.history, motion, player1.GameField, player1.motionColor);

            Assert.AreEqual(player1.king.Offset.Row, 6);
            Assert.AreEqual(player1.king.Offset.Column, 5);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода minMax. Позиция для хода к1 = (6, 5). 4-(-200) = 204. Проверка позиции
        /// </summary>
        public void getMinMaxTest1CheckPosition()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;
            player1.king.MoveFigure(5, 5);
            player1.queen.MoveFigure(7, 0);
            player2.king.MoveFigure(4, 1);
            player2.queen.MoveFigure(0, 0);

            GameField[1, 1] = -5; // стена
            GameField[5, 0] = -5;
            GameField[6, 0] = -5;
            GameField[6, 1] = -5;

            //DynamicField.minMax(player1, 1, player1.king.Offset, player1.queen.Offset, player2.king.Offset, player2.queen.Offset, int.MinValue, int.MaxValue, 1, player1.history, motion, player1.GameField, player1.motionColor);
            Assert.AreEqual(6, player1.king.Offset.Row);
            Assert.AreEqual(5, player1.king.Offset.Column);

             //DynamicField.minMax(player2, 1, player1.king.Offset, player1.queen.Offset, player2.king.Offset, player2.queen.Offset, int.MinValue, int.MaxValue, 1, player2.history, motion, player2.GameField, player2.motionColor);
            Assert.AreEqual(0, player2.queen.Offset.Row);
            Assert.AreEqual(4, player2.queen.Offset.Column);

            //DynamicField.minMax(player1, 1, player1.king.Offset, player1.queen.Offset, player2.king.Offset, player2.queen.Offset, int.MinValue, int.MaxValue, 1, player1.history, motion, player1.GameField, player1.motionColor);
            Assert.AreEqual(6, player1.king.Offset.Row);
            Assert.AreEqual(5, player1.king.Offset.Column);
            Assert.AreEqual(7, player1.queen.Offset.Row);
            Assert.AreEqual(1, player1.queen.Offset.Column);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода minMax. Позиция для хода к1 = (6, 5). 4-(-200) = 204. Проверка позиции. Level 2.
        /// </summary>
        public void getMinMaxTest3Level2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;
            player1.king.MoveFigure(5, 5);
            player1.queen.MoveFigure(7, 0);
            player2.king.MoveFigure(4, 1);
            player2.queen.MoveFigure(0, 0);

            GameField[1, 1] = -5; // стена
            GameField[5, 0] = -5;
            GameField[6, 0] = -5;
            GameField[6, 1] = -5;

            //DynamicField.minMax(player1, 2, player1.king.Offset, player1.queen.Offset, player2.king.Offset, player2.queen.Offset, int.MinValue, int.MaxValue, 2, player2.history, motion, player2.GameField, player2.motionColor);

            Assert.AreEqual(6, player1.king.Offset.Row);
            Assert.AreEqual(4, player1.king.Offset.Column); // 5?
        }


        [TestMethod]
        /// <summary>
        /// Тест для проверки метода isCorridor
        /// </summary>
        public void IsCorridorTest1()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            GameField[1, 5] = -5; // стена
            GameField[2, 5] = -5;
            GameField[3, 1] = -5;
            GameField[3, 5] = -5;
            GameField[3, 6] = -5;
            GameField[5, 0] = -5;
            GameField[5, 3] = -5;
            GameField[5, 4] = -5;
            GameField[5, 6] = -5;
            GameField[6, 2] = -5;
            GameField[7, 4] = -5;
            GameField[7, 6] = -5;
            GameField[7, 7] = -5;

            Assert.AreEqual(true, GameField.IsCorridor(3, 0));
            Assert.AreEqual(true, GameField.IsCorridor(3, 7));
            Assert.AreEqual(true, GameField.IsCorridor(5, 1));
            Assert.AreEqual(true, GameField.IsCorridor(5, 2));
            Assert.AreEqual(true, GameField.IsCorridor(5, 5));
            Assert.AreEqual(true, GameField.IsCorridor(5, 7));
            Assert.AreEqual(true, GameField.IsCorridor(7, 5));
            Assert.AreEqual(true, GameField.IsCorridor(6, 0));
            Assert.AreEqual(true, GameField.IsCorridor(2, 7));

            Assert.AreEqual(false, GameField.IsCorridor(1, 0));
            Assert.AreEqual(false, GameField.IsCorridor(1, 1));
            Assert.AreEqual(false, GameField.IsCorridor(1, 2));
            Assert.AreEqual(false, GameField.IsCorridor(1, 3));
            Assert.AreEqual(false, GameField.IsCorridor(1, 4));
            Assert.AreEqual(false, GameField.IsCorridor(3, 2));
            Assert.AreEqual(false, GameField.IsCorridor(3, 3));
            Assert.AreEqual(false, GameField.IsCorridor(3, 4));
            Assert.AreEqual(false, GameField.IsCorridor(4, 0));
            Assert.AreEqual(false, GameField.IsCorridor(4, 1));
            Assert.AreEqual(false, GameField.IsCorridor(4, 2));
            Assert.AreEqual(false, GameField.IsCorridor(6, 5));
            Assert.AreEqual(false, GameField.IsCorridor(6, 6));
            Assert.AreEqual(false, GameField.IsCorridor(6, 7));
            Assert.AreEqual(false, GameField.IsCorridor(7, 1));
            Assert.AreEqual(false, GameField.IsCorridor(7, 2));
            Assert.AreEqual(false, GameField.IsCorridor(7, 3));
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода isCorridor
        /// </summary>
        public void IsCorridorTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            GameField[1, 5] = -5; // стена
            GameField[2, 5] = -5;
            GameField[3, 1] = -5;
            GameField[3, 5] = -5;
            GameField[3, 6] = -5;
            GameField[5, 0] = -5;
            GameField[5, 3] = -5;
            GameField[5, 4] = -5;
            GameField[5, 6] = -5;
            GameField[6, 2] = -5;
            GameField[7, 4] = -5;
            GameField[7, 6] = -5;
            GameField[7, 7] = -5;

            Assert.AreEqual(true, GameField.IsCorridor(6, 1));
            Assert.AreEqual(true, GameField.IsCorridor(2, 6));
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода isCorridor
        /// </summary>
        public void IsCorridorTest3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            GameField[1, 5] = -5; // стена
            GameField[2, 5] = -5;
            GameField[3, 1] = -5;
            GameField[3, 5] = -5;
            GameField[3, 6] = -5;
            GameField[5, 0] = -5;
            GameField[5, 3] = -5;
            GameField[5, 4] = -5;
            GameField[5, 6] = -5;
            GameField[6, 2] = -5;
            GameField[7, 4] = -5;
            GameField[7, 6] = -5;
            GameField[7, 7] = -5;

            Assert.AreEqual(true, GameField.IsCorridor(1, 6));
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода isCorridor
        /// </summary>
        public void IsCorridorTest4()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            GameField[1, 4] = -5;
            GameField[5, 2] = -5;
            GameField[5, 4] = -5;
            GameField[5, 7] = -5;
            GameField[2, 2] = -5;
            GameField[3, 1] = -5;
            GameField[3, 2] = -5;
            GameField[0, 2] = -5;
            GameField[0, 3] = -5;
            GameField[0, 6] = -5;
            GameField[0, 7] = -5;
            GameField[7, 2] = -5;
            GameField[7, 3] = -5;
            GameField[7, 5] = -5;
            GameField[7, 6] = -5;

            Assert.AreEqual(true, GameField.IsCorridor(5, 3));
            Assert.AreEqual(true, GameField.IsCorridor(5, 5));
            Assert.AreEqual(true, GameField.IsCorridor(5, 6));
            Assert.AreEqual(false, GameField.IsCorridor(1, 5));
            Assert.AreEqual(false, GameField.IsCorridor(1, 6));
            Assert.AreEqual(false, GameField.IsCorridor(1, 7));
            Assert.AreEqual(true, GameField.IsCorridor(2, 0));
            Assert.AreEqual(true, GameField.IsCorridor(3, 0));
            Assert.AreEqual(false, GameField.IsCorridor(0, 4)); // король стоит на этом месте
            Assert.AreEqual(true, GameField.IsCorridor(0, 5));
            Assert.AreEqual(false, GameField.IsCorridor(7, 4)); // король стоит на этом месте
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода isCorridor
        /// </summary>
        public void IsCorridorTest5()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveFigure(3, 1);
            player1.queen.MoveFigure(5, 3);
            player2.king.MoveFigure(1, 2);
            player2.queen.MoveFigure(7, 3);

            GameField[3, 5] = -5;
            GameField[7, 6] = -5;
            GameField[1, 7] = -5;

            Assert.AreEqual(false, GameField.IsCorridor(1, 3));
            Assert.AreEqual(false, GameField.IsCorridor(1, 4));
            Assert.AreEqual(false, GameField.IsCorridor(1, 5));
            Assert.AreEqual(false, GameField.IsCorridor(1, 6));
            Assert.AreEqual(false, GameField.IsCorridor(1, 7)); // стена не является коридором
            Assert.AreEqual(true, GameField.IsCorridor(3, 0));
            Assert.AreEqual(false, GameField.IsCorridor(3, 2));
            Assert.AreEqual(false, GameField.IsCorridor(3, 3));
            Assert.AreEqual(false, GameField.IsCorridor(3, 4));
            Assert.AreEqual(false, GameField.IsCorridor(3, 5)); // стена не является коридором
            Assert.AreEqual(true, GameField.IsCorridor(3, 6));
            Assert.AreEqual(true, GameField.IsCorridor(3, 7));
            Assert.AreEqual(false, GameField.IsCorridor(5, 0));
            Assert.AreEqual(false, GameField.IsCorridor(5, 1));
            Assert.AreEqual(false, GameField.IsCorridor(5, 2));
            Assert.AreEqual(false, GameField.IsCorridor(5, 3)); // фигура не является коридором
            Assert.AreEqual(false, GameField.IsCorridor(5, 4));
            Assert.AreEqual(false, GameField.IsCorridor(5, 5));
            Assert.AreEqual(false, GameField.IsCorridor(5, 6));
            Assert.AreEqual(false, GameField.IsCorridor(5, 7));
            Assert.AreEqual(false, GameField.IsCorridor(7, 0));
            Assert.AreEqual(false, GameField.IsCorridor(7, 1));
            Assert.AreEqual(false, GameField.IsCorridor(7, 2));
            Assert.AreEqual(true, GameField.IsCorridor(7, 4));
            Assert.AreEqual(true, GameField.IsCorridor(7, 5));
            Assert.AreEqual(true, GameField.IsCorridor(7, 7));
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода isCorridor
        /// </summary>
        public void IsCorridorTest6()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            GameField[1, 1] = -5; // стена
            GameField[1, 3] = -5;
            GameField[1, 6] = -5;
            GameField[2, 3] = -5;
            GameField[2, 5] = -5;
            GameField[2, 6] = -5;
            GameField[2, 7] = -5;

            Assert.AreEqual(true, GameField.IsCorridor(1, 0));
            Assert.AreEqual(true, GameField.IsCorridor(1, 2));
            Assert.AreEqual(true, GameField.IsCorridor(1, 4));
            Assert.AreEqual(true, GameField.IsCorridor(1, 5));
            Assert.AreEqual(true, GameField.IsCorridor(1, 7));
            Assert.AreEqual(false, GameField.IsCorridor(2, 0));
            Assert.AreEqual(false, GameField.IsCorridor(2, 1));
            Assert.AreEqual(false, GameField.IsCorridor(2, 2));
            Assert.AreEqual(false, GameField.IsCorridor(2, 3));
            Assert.AreEqual(true, GameField.IsCorridor(2, 4));
            Assert.AreEqual(false, GameField.IsCorridor(2, 7));
            Assert.AreEqual(false, GameField.IsCorridor(2, 6));
            Assert.AreEqual(false, GameField.IsCorridor(2, 5));
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода HasConfigurationOccurredFiveTimes
        /// </summary> 
        public void HasConfigurationOccurredFiveTimesTest1()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            // ход 1
            player1.king.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.king.Id, new Position(1, 3)));
            player2.king.MoveFigure(6, 2);
            player2.history.Add(motion, (player2.king.Id, new Position(6, 2)));

            Assert.AreEqual(false, GameField.HasConfigurationOccurredFiveTimes(player1.history, player2.history));
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода HasConfigurationOccurredFiveTimes. Конфигурация повторялась 5 раз.
        /// </summary> 
        public void HasConfigurationOccurredFiveTimesTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            // ход 1
            player1.king.MoveFigure(1, 4);
            player1.history.Add(motion, (player1.king.Id, new Position(1, 4)));
            player2.king.MoveFigure(6, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(6, 5)));
            motion++;

            // ход 2
            player1.king.MoveFigure(2, 5);
            player1.history.Add(motion, (player1.king.Id, new Position(2, 5)));
            player2.king.MoveFigure(5, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 5)));
            motion++;

            // ход 3
            player1.king.MoveFigure(3, 6);
            player1.history.Add(motion, (player1.king.Id, new Position(3, 6)));
            player2.king.MoveFigure(5, 6);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 6)));
            motion++;

            // ход 4 конфигурация
            player1.queen.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 3)));
            player2.queen.MoveFigure(6, 2);
            player2.history.Add(motion, (player2.queen.Id, new Position(6, 2)));
            motion++;

            // ход 5
            player1.queen.MoveFigure(3, 1);
            player1.history.Add(motion, (player1.queen.Id, new Position(3, 1)));
            player2.queen.MoveFigure(7, 1);
            player2.history.Add(motion, (player2.queen.Id, new Position(7, 1)));
            motion++;

            // ход 6 повтор конфигурации (1)
            player1.queen.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 3)));
            player2.queen.MoveFigure(6, 2);
            player2.history.Add(motion, (player2.queen.Id, new Position(6, 2)));
            motion++;

            // ход 7
            player1.king.MoveFigure(2, 5);
            player1.history.Add(motion, (player1.king.Id, new Position(2, 5)));
            player2.king.MoveFigure(5, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 5)));
            motion++;

            // ход 8 повтор конфигурации (2)
            player1.king.MoveFigure(3, 6);
            player1.history.Add(motion, (player1.king.Id, new Position(3, 6)));
            player2.king.MoveFigure(5, 6);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 6)));
            motion++;

            // ход 9
            player1.queen.MoveFigure(3, 1);
            player1.history.Add(motion, (player1.queen.Id, new Position(3, 1)));
            player2.queen.MoveFigure(7, 1);
            player2.history.Add(motion, (player2.queen.Id, new Position(7, 1)));
            motion++;

            // ход 10 повтор конфигурации (3)
            player1.queen.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 3)));
            player2.queen.MoveFigure(6, 2);
            player2.history.Add(motion, (player2.queen.Id, new Position(6, 2)));
            motion++;

            // ход 11
            player1.king.MoveFigure(2, 5);
            player1.history.Add(motion, (player1.king.Id, new Position(2, 5)));
            player2.king.MoveFigure(5, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 5)));
            motion++;

            // ход 12 повтор конфигурации (4)
            player1.king.MoveFigure(3, 6);
            player1.history.Add(motion, (player1.king.Id, new Position(3, 6)));
            player2.king.MoveFigure(5, 6);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 6)));
            motion++;

            // ход 13
            player1.queen.MoveFigure(3, 1);
            player1.history.Add(motion, (player1.queen.Id, new Position(3, 1)));
            player2.queen.MoveFigure(7, 1);
            player2.history.Add(motion, (player2.queen.Id, new Position(7, 1)));
            motion++;

            // ход 14 повтор конфигурации (5)
            player1.queen.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 3)));
            player2.queen.MoveFigure(6, 2);
            player2.history.Add(motion, (player2.queen.Id, new Position(6, 2)));

            Assert.AreEqual(true, GameField.HasConfigurationOccurredFiveTimes(player1.history, player2.history));
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода HasConfigurationOccurredFiveTimes. Конфигурация повторялась 4 раза.
        /// </summary> 
        public void HasConfigurationOccurredFiveTimesTest3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            // ход 1
            player1.king.MoveFigure(1, 4);
            player1.history.Add(motion, (player1.king.Id, new Position(1, 4)));
            player2.king.MoveFigure(6, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(6, 5)));
            motion++;

            // ход 2
            player1.king.MoveFigure(2, 5);
            player1.history.Add(motion, (player1.king.Id, new Position(2, 5)));
            player2.king.MoveFigure(5, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 5)));
            motion++;

            // ход 3
            player1.king.MoveFigure(3, 6);
            player1.history.Add(motion, (player1.king.Id, new Position(3, 6)));
            player2.king.MoveFigure(5, 6);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 6)));
            motion++;

            // ход 4 конфигурация
            player1.queen.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 3)));
            player2.queen.MoveFigure(6, 2);
            player2.history.Add(motion, (player2.queen.Id, new Position(6, 2)));
            motion++;

            // ход 5
            player1.queen.MoveFigure(3, 1);
            player1.history.Add(motion, (player1.queen.Id, new Position(3, 1)));
            player2.queen.MoveFigure(7, 1);
            player2.history.Add(motion, (player2.queen.Id, new Position(7, 1)));
            motion++;

            // ход 6 повтор конфигурации (1)
            player1.queen.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 3)));
            player2.queen.MoveFigure(6, 2);
            player2.history.Add(motion, (player2.queen.Id, new Position(6, 2)));
            motion++;

            // ход 7
            player1.king.MoveFigure(2, 5);
            player1.history.Add(motion, (player1.king.Id, new Position(2, 5)));
            player2.king.MoveFigure(5, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 5)));
            motion++;

            // ход 8 повтор конфигурации (2)
            player1.king.MoveFigure(3, 6);
            player1.history.Add(motion, (player1.king.Id, new Position(3, 6)));
            player2.king.MoveFigure(5, 6);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 6)));
            motion++;

            // ход 9
            player1.queen.MoveFigure(3, 1);
            player1.history.Add(motion, (player1.queen.Id, new Position(3, 1)));
            player2.queen.MoveFigure(7, 1);
            player2.history.Add(motion, (player2.queen.Id, new Position(7, 1)));
            motion++;

            // ход 10 повтор конфигурации (3)
            player1.queen.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 3)));
            player2.queen.MoveFigure(6, 2);
            player2.history.Add(motion, (player2.queen.Id, new Position(6, 2)));
            motion++;

            // ход 11
            player1.king.MoveFigure(2, 5);
            player1.history.Add(motion, (player1.king.Id, new Position(2, 5)));
            player2.king.MoveFigure(5, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 5)));
            motion++;

            // ход 12 повтор конфигурации (4)
            player1.king.MoveFigure(3, 6);
            player1.history.Add(motion, (player1.king.Id, new Position(3, 6)));
            player2.king.MoveFigure(5, 6);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 6)));
            motion++;

            Assert.AreEqual(true, GameField.HasConfigurationOccurredFiveTimes(player1.history, player2.history));
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода HasConfigurationOccurredFiveTimes. Конфигурация повторялась 3 раза.
        /// </summary> 
        public void HasConfigurationOccurredFiveTimesTest6()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            // ход 1
            player1.king.MoveFigure(1, 4);
            player1.history.Add(motion, (player1.king.Id, new Position(1, 4)));
            player2.king.MoveFigure(6, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(6, 5)));
            motion++;

            // ход 2
            player1.king.MoveFigure(2, 5);
            player1.history.Add(motion, (player1.king.Id, new Position(2, 5)));
            player2.king.MoveFigure(5, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 5)));
            motion++;

            // ход 3
            player1.king.MoveFigure(3, 6);
            player1.history.Add(motion, (player1.king.Id, new Position(3, 6)));
            player2.king.MoveFigure(5, 6);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 6)));
            motion++;

            // ход 4 конфигурация
            player1.queen.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 3)));
            player2.queen.MoveFigure(6, 2);
            player2.history.Add(motion, (player2.queen.Id, new Position(6, 2)));
            motion++;

            // ход 5
            player1.queen.MoveFigure(3, 1);
            player1.history.Add(motion, (player1.queen.Id, new Position(3, 1)));
            player2.queen.MoveFigure(7, 1);
            player2.history.Add(motion, (player2.queen.Id, new Position(7, 1)));
            motion++;

            // ход 6 повтор конфигурации (1)
            player1.queen.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 3)));
            player2.queen.MoveFigure(6, 2);
            player2.history.Add(motion, (player2.queen.Id, new Position(6, 2)));
            motion++;

            // ход 7
            player1.king.MoveFigure(2, 5);
            player1.history.Add(motion, (player1.king.Id, new Position(2, 5)));
            player2.king.MoveFigure(5, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 5)));
            motion++;

            // ход 8 повтор конфигурации (2)
            player1.king.MoveFigure(3, 6);
            player1.history.Add(motion, (player1.king.Id, new Position(3, 6)));
            player2.king.MoveFigure(5, 6);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 6)));
            motion++;

            // ход 9
            player1.queen.MoveFigure(3, 1);
            player1.history.Add(motion, (player1.queen.Id, new Position(3, 1)));
            player2.queen.MoveFigure(7, 1);
            player2.history.Add(motion, (player2.queen.Id, new Position(7, 1)));
            motion++;

            // ход 10 повтор конфигурации (3)
            player1.queen.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 3)));
            player2.queen.MoveFigure(6, 2);
            player2.history.Add(motion, (player2.queen.Id, new Position(6, 2)));
            motion++;

            Assert.AreEqual(false, GameField.HasConfigurationOccurredFiveTimes(player1.history, player2.history));
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода HasConfigurationOccurredFiveTimes. Конфигурация не повторялась.
        /// </summary> 
        public void HasConfigurationOccurredFiveTimesTest4()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            // ход 1
            player1.king.MoveFigure(1, 4);
            player1.history.Add(motion, (player1.king.Id, new Position(1, 4)));
            player2.king.MoveFigure(6, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(6, 5)));
            motion++;

            // ход 2
            player1.king.MoveFigure(2, 5);
            player1.history.Add(motion, (player1.king.Id, new Position(2, 5)));
            player2.king.MoveFigure(5, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 5)));
            motion++;

            // ход 3
            player1.king.MoveFigure(3, 6);
            player1.history.Add(motion, (player1.king.Id, new Position(3, 6)));
            player2.king.MoveFigure(5, 6);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 6)));
            motion++;

            // ход 4 конфигурация
            player1.queen.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 3)));
            player2.queen.MoveFigure(6, 2);
            player2.history.Add(motion, (player2.queen.Id, new Position(6, 2)));

            Assert.AreEqual(false, GameField.HasConfigurationOccurredFiveTimes(player1.history, player2.history));
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода HasConfigurationOccurredFiveTimes. Конфигурация повторилась 1 раз.
        /// </summary> 
        public void HasConfigurationOccurredFiveTimesTest5()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;
            int motion = 1;

            // ход 1
            player1.king.MoveFigure(1, 4);
            player1.history.Add(motion, (player1.king.Id, new Position(1, 4)));
            player2.king.MoveFigure(6, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(6, 5)));
            motion++;

            // ход 2
            player1.king.MoveFigure(2, 5);
            player1.history.Add(motion, (player1.king.Id, new Position(2, 5)));
            player2.king.MoveFigure(5, 5);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 5)));
            motion++;

            // ход 3
            player1.king.MoveFigure(3, 6);
            player1.history.Add(motion, (player1.king.Id, new Position(3, 6)));
            player2.king.MoveFigure(5, 6);
            player2.history.Add(motion, (player2.king.Id, new Position(5, 6)));
            motion++;

            // ход 4 конфигурация
            player1.queen.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 3)));
            player2.queen.MoveFigure(6, 2);
            player2.history.Add(motion, (player2.queen.Id, new Position(6, 2)));
            motion++;

            // ход 5
            player1.queen.MoveFigure(3, 1);
            player1.history.Add(motion, (player1.queen.Id, new Position(3, 1)));
            player2.queen.MoveFigure(7, 1);
            player2.history.Add(motion, (player2.queen.Id, new Position(7, 1)));
            motion++;

            // ход 6 повтор конфигурации (1)
            player1.queen.MoveFigure(1, 3);
            player1.history.Add(motion, (player1.queen.Id, new Position(1, 3)));
            player2.queen.MoveFigure(6, 2);
            player2.history.Add(motion, (player2.queen.Id, new Position(6, 2)));
            motion++;

            Assert.AreEqual(false, GameField.HasConfigurationOccurredFiveTimes(player1.history, player2.history));
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getAllCoridorPositions. (для белого короля)
        /// </summary>
        public void getAllCoridorPositionsTest1()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            GameField[1, 1] = -5; // стена
            GameField[1, 3] = -5;
            GameField[1, 6] = -5;
            GameField[2, 3] = -5;
            GameField[2, 5] = -5;
            GameField[2, 6] = -5;
            GameField[2, 7] = -5;

            GameField[5, 0] = -5; // стена
            GameField[5, 2] = -5;
            GameField[5, 4] = -5;
            GameField[5, 6] = -5;
            GameField[6, 0] = -5;
            GameField[6, 1] = -5;
            GameField[6, 2] = -5;
            GameField[6, 5] = -5;
            GameField[6, 6] = -5;

            List<Position> positions = player1.king.GetAllCoridorPositions(player1.Сompetitor.queen, player1.Сompetitor.king);
            List<Position> list = new List<Position>() { new Position(1, 0), new Position(1, 2), new Position(1, 4), new Position(1, 5), new Position(1, 7), new Position(2, 4) };
            List<Position> positionsForBlackKing = player2.king.GetAllCoridorPositions(player2.Сompetitor.queen, player2.Сompetitor.king);
            List<Position> listForBlackKing = new List<Position>() { new Position(6, 3), new Position(6, 4), new Position(6, 7), new Position(5, 1), new Position(5, 3), new Position(5, 5), new Position(5, 7) };
            CollectionAssert.AreEqual(list, positions);
            CollectionAssert.AreEqual(listForBlackKing, positionsForBlackKing);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getAllCoridorPositions. (для белого короля)
        /// </summary>
        public void getAllCoridorPositionsTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveFigure(3, 1);
            player2.king.MoveFigure(5, 5);
            player2.queen.MoveFigure(7, 2);

            GameField[3, 2] = -5; // стена
            GameField[3, 4] = -5;
            GameField[4, 2] = -5;
            GameField[4, 5] = -5;
            GameField[5, 3] = -5;
            GameField[5, 6] = -5;

            List<Position> positions = player1.king.GetAllCoridorPositions(player1.Сompetitor.queen, player1.Сompetitor.king);
            List<Position> list = new List<Position>() { new Position(4, 0), new Position(4, 1), new Position(4, 3), new Position(4, 7), new Position(5, 7)};
            List<Position> positionsForBlackKing = player2.king.GetAllCoridorPositions(player2.Сompetitor.queen, player2.Сompetitor.king);
            List<Position> listForBlackKing = new List<Position>() { new Position(4, 4), new Position(4, 6)};
            CollectionAssert.AreEqual(list, positions);
            CollectionAssert.AreEqual(listForBlackKing, positionsForBlackKing);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода getAllCoridorPositions. (для белого короля)
        /// </summary>
        public void getAllCoridorPositionsTest3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveFigure(7, 2);
            player2.king.MoveFigure(1, 3);
            player1.queen.MoveFigure(2, 0);
            player2.queen.MoveFigure(5, 7);

            GameField[0, 3] = -5; // стена
            GameField[0, 5] = -5;
            GameField[0, 6] = -5;

            List<Position> positions = player1.king.GetAllCoridorPositions(player1.Сompetitor.queen, player1.Сompetitor.king);
            List<Position> list = new List<Position>();
            List<Position> positionsForBlackKing = player2.king.GetAllCoridorPositions(player2.Сompetitor.queen, player2.Сompetitor.king);
            List<Position> listForBlackKing = new List<Position>() { new Position(0, 4), new Position(0, 7)};
            CollectionAssert.AreEqual(list, positions);
            CollectionAssert.AreEqual(listForBlackKing, positionsForBlackKing);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода findNearestPointCorridorStartegyTest1.
        /// </summary>
        public void FindNearestPointCorridorStartegyTest1()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveFigure(3, 1);
            player2.king.MoveFigure(5, 5);
            player2.queen.MoveFigure(7, 2);

            GameField[3, 2] = -5; // стена
            GameField[3, 4] = -5;
            GameField[4, 2] = -5;
            GameField[4, 5] = -5;
            GameField[5, 3] = -5;
            GameField[5, 6] = -5;

            Position pos = player1.king.FindNearestPointCorridorStartegy(player1.Сompetitor.queen, player1.Сompetitor.king);
            Assert.AreEqual(4, pos.Row);
            Assert.AreEqual(0, pos.Column);

            Position pos2 = player2.king.FindNearestPointCorridorStartegy(player2.Сompetitor.queen, player2.Сompetitor.king);
            Assert.AreEqual(4, pos2.Row);
            Assert.AreEqual(4, pos2.Column);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода findNearestPointCorridorStartegyTest1.
        /// </summary>
        public void findNearestPointCorridorStartegyTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveFigure(1, 4);
            player2.king.MoveFigure(4, 5);
            player2.queen.MoveFigure(6, 2);

            GameField[2, 2] = -5;
            GameField[2, 3] = -5;
            GameField[2, 5] = -5;
            GameField[2, 7] = -5;
            GameField[3, 1] = -5;
            GameField[3, 2] = -5;
            GameField[4, 1] = -5;
            GameField[4, 3] = -5;
            GameField[4, 4] = -5;
            GameField[4, 7] = -5;
            GameField[5, 5] = -5;
            GameField[5, 6] = -5;

            Position pos = player1.king.FindNearestPointCorridorStartegy(player1.Сompetitor.queen, player1.Сompetitor.king);
            Assert.AreEqual(2, pos.Row);
            Assert.AreEqual(4, pos.Column);

            Position pos2 = player2.king.FindNearestPointCorridorStartegy(player2.Сompetitor.queen, player2.Сompetitor.king);
            Assert.AreEqual(2, pos2.Row);
            Assert.AreEqual(6, pos2.Column);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода findNearestPointCorridorStartegyTest1.
        /// </summary>
        public void findNearestPointCorridorStartegyTest3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveFigure(2, 6);
            player2.king.MoveFigure(4, 5);
            player1.queen.MoveFigure(2, 0);

            GameField[3, 2] = -5;
            GameField[3, 3] = -5;
            GameField[3, 4] = -5;

            Position pos = player2.king.FindNearestPointCorridorStartegy(player2.Сompetitor.queen, player2.Сompetitor.king);
            Assert.AreEqual(-1, pos.Row);
            Assert.AreEqual(-1, pos.Column);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода
        /// </summary>
        public void StartegyCorridorTest()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            GameField[0, 6] = -5;
            GameField[0, 7] = -5;

            GameField[1, 1] = -5;
            GameField[1, 4] = -5;
            GameField[1, 6] = -5;

            GameField[2, 0] = -5;
            GameField[2, 1] = -5;
            GameField[2, 4] = -5;
            GameField[2, 6] = -5;
            GameField[2, 7] = -5;

            GameField[3, 0] = -5;
            GameField[3, 1] = -5;
            GameField[3, 6] = -5;
            GameField[3, 7] = -5;

            GameField[4, 1] = -5;
            GameField[4, 5] = -5;
            GameField[4, 7] = -5;

            GameField[5, 1] = -5;
            GameField[5, 2] = -5;
            GameField[5, 3] = -5;
            GameField[5, 5] = -5;

            GameField[6, 0] = -5;
            GameField[6, 1] = -5;
            GameField[6, 5] = -5;
            GameField[6, 7] = -5;

            GameField[6, 0] = -5;
            GameField[6, 1] = -5;
            GameField[6, 5] = -5;
            GameField[6, 7] = -5;

            GameField[7, 0] = -5;
            GameField[7, 2] = -5;
            GameField[7, 6] = -5;
            GameField[7, 7] = -5;

            Position pos = player1.king.FindNearestPointCorridorStartegy(player1.Сompetitor.queen, player1.Сompetitor.king);
            Assert.AreEqual(1, pos.Row);
            Assert.AreEqual(3, pos.Column);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода
        /// </summary>
        public void StartegyCorridorTest2()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveFigure(2, 6);
            player2.king.MoveFigure(5, 3);
            player1.queen.MoveFigure(0, 3);
            player2.queen.MoveFigure(7, 3);

            GameField[0, 1] = -5;
            GameField[0, 2] = -5;

            GameField[1, 0] = -5;
            GameField[1, 4] = -5;
            GameField[1, 6] = -5;

            GameField[2, 0] = -5;
            GameField[2, 1] = -5;
            GameField[2, 3] = -5;
            GameField[2, 4] = -5;
            GameField[2, 5] = -5;
            GameField[2, 7] = -5;

            GameField[3, 0] = -5;
            GameField[3, 1] = -5;
            GameField[3, 6] = -5;

            GameField[4, 1] = -5;
            GameField[4, 3] = -5;
            GameField[4, 4] = -5;
            GameField[4, 5] = -5;
            GameField[4, 7] = -5;

            GameField[5, 1] = -5;
            GameField[5, 2] = -5;
            GameField[5, 7] = -5;

            GameField[6, 1] = -5;
            GameField[6, 2] = -5;
            GameField[6, 3] = -5;
            GameField[6, 6] = -5;

            GameField[7, 1] = -5;
            GameField[7, 2] = -5;
            GameField[7, 6] = -5;
            GameField[7, 7] = -5;

            int result = player1.king.MoveKingCorridorStartegy(1, player1.posEnd, player1.Сompetitor.queen, player1.Сompetitor.king, player1.history, 1, player1.queen);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода
        /// </summary>
        public void StartegyCorridorTest3()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            GameField[1, 0] = -5;
            GameField[1, 1] = -5;
            GameField[2, 1] = -5;
            GameField[3, 1] = -5;
            GameField[4, 0] = -5;
            GameField[4, 1] = -5;

            GameField[6, 0] = -5;
            GameField[6, 1] = -5;
            GameField[7, 1] = -5;

            GameField[6, 6] = -5;
            GameField[7, 6] = -5;

            GameField[2, 4] = -5;
            GameField[2, 5] = -5;
            GameField[2, 6] = -5;
            GameField[3, 4] = -5;
            GameField[3, 6] = -5;
            GameField[4, 4] = -5;
            GameField[4, 5] = -5;
            GameField[4, 6] = -5;


            bool res = GameField.IsIsolatedPositionIndividual(2, 0);
            Assert.AreEqual(true, res);
            Assert.AreEqual(true, GameField.IsIsolatedPositionIndividual(3, 0));
            Assert.AreEqual(true, GameField.IsIsolatedPositionIndividual(7, 0));
            Assert.AreEqual(false, GameField.IsIsolatedPositionIndividual(5, 0));
            Assert.AreEqual(true, GameField.IsIsolatedPositionIndividual(3, 5));
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода
        /// </summary>
        public void StartegyCorridorTest4()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            GameField[1, 0] = -5;
            GameField[1, 1] = -5;
            GameField[2, 1] = -5;
            GameField[3, 1] = -5;
            GameField[4, 0] = -5;
            GameField[4, 1] = -5;

            GameField[6, 0] = -5;
            GameField[6, 1] = -5;
            GameField[7, 1] = -5;

            GameField[6, 6] = -5;
            GameField[7, 6] = -5;

            GameField[2, 4] = -5;
            GameField[2, 5] = -5;
            GameField[2, 6] = -5;
            GameField[3, 4] = -5;
            GameField[3, 6] = -5;
            GameField[4, 4] = -5;
            GameField[4, 5] = -5;
            GameField[4, 6] = -5;


            bool res = GameField.IsIsolatedPositionIndividual(2, 0);
            Assert.AreEqual(true, res);
            Assert.AreEqual(true, GameField.IsIsolatedPositionIndividual(3, 0));
            Assert.AreEqual(true, GameField.IsIsolatedPositionIndividual(7, 0));
            Assert.AreEqual(false, GameField.IsIsolatedPositionIndividual(5, 0));
            Assert.AreEqual(true, GameField.IsIsolatedPositionIndividual(3, 5));
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода
        /// </summary>
        public void StartegyCorridorTest5()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            GameField[0, 0] = -5;
            GameField[0, 1] = -5;
            GameField[0, 2] = -5;
            GameField[0, 5] = -5;
            GameField[0, 6] = -5;
            GameField[0, 7] = -5;

            GameField[1, 1] = -5;
            GameField[1, 2] = -5;
            GameField[1, 3] = -5;

            GameField[2, 0] = -5;
            GameField[2, 1] = -5;
            GameField[2, 2] = -5;
            GameField[2, 4] = -5;
            GameField[2, 7] = -5;

            GameField[3, 0] = -5;
            GameField[3, 2] = -5;
            GameField[3, 3] = -5;
            GameField[3, 6] = -5;
            GameField[3, 7] = -5;

            GameField[4, 0] = -5;

            GameField[5, 0] = -5;
            GameField[5, 2] = -5;
            GameField[5, 6] = -5;
            GameField[5, 7] = -5;

            GameField[6, 0] = -5;
            GameField[6, 2] = -5;
            GameField[6, 3] = -5;
            GameField[6, 6] = -5;
            GameField[6, 7] = -5;
            GameField[7, 2] = -5;

            Position pos = player1.king.FindNearestPointCorridorStartegy(player1.Сompetitor.queen, player1.Сompetitor.king);
            Assert.AreEqual(2, pos.Row);
            Assert.AreEqual(3, pos.Column);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода
        /// </summary>
        public void StartegyCorridorTest6()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveFigure(2, 3);
            player2.king.MoveFigure(5, 5);

            GameField[0, 7] = -5;

            GameField[1, 0] = -5;
            GameField[1, 2] = -5;
            GameField[1, 4] = -5;
            GameField[1, 5] = -5;

            GameField[2, 0] = -5;
            GameField[2, 1] = -5;
            GameField[2, 2] = -5;

            GameField[3, 1] = -5;
            GameField[3, 3] = -5;
            GameField[3, 4] = -5;

            GameField[4, 2] = -5;
            GameField[4, 3] = -5;
            GameField[4, 4] = -5;
            GameField[4, 7] = -5;

            GameField[5, 0] = -5;
            GameField[5, 1] = -5;
            GameField[5, 2] = -5;
            GameField[5, 3] = -5;
            GameField[5, 4] = -5;
            GameField[5, 7] = -5;

            GameField[6, 0] = -5;
            GameField[6, 1] = -5;
            GameField[6, 2] = -5;
            GameField[6, 3] = -5;

            GameField[7, 0] = -5;
            GameField[7, 1] = -5;
            GameField[7, 2] = -5;
            GameField[7, 5] = -5;
            GameField[7, 7] = -5;


            Position pos = player1.king.FindNearestPointCorridorStartegy(player1.Сompetitor.queen, player1.Сompetitor.king);
            Assert.AreEqual(-1, pos.Row); // если уйдет на (3, 2), то заблокирует себя
            Assert.AreEqual(-1, pos.Column);
        }


        [TestMethod]
        /// <summary>
        /// Тест для проверки метода
        /// </summary>
        public void StartegyCorridorTest7()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveFigure(1, 6);
            player2.king.MoveFigure(7, 7);
            player1.queen.MoveFigure(1, 4);
            player2.queen.MoveFigure(4, 7);

            GameField[0, 0] = -5;
            GameField[0, 1] = -5;
            GameField[0, 6] = -5;

            GameField[1, 1] = -5;
            GameField[1, 2] = -5;
            GameField[1, 3] = -5;
            GameField[1, 5] = -5;

            GameField[2, 0] = -5;
            GameField[2, 4] = -5;
            GameField[2, 5] = -5;
            GameField[2, 6] = -5;

            GameField[3, 0] = -5;
            GameField[3, 2] = -5;
            GameField[3, 3] = -5;
            GameField[3, 6] = -5;

            GameField[4, 0] = -5;
            GameField[4, 2] = -5;
            GameField[4, 3] = -5;
            GameField[4, 4] = -5;
            GameField[4, 6] = -5;

            GameField[5, 1] = -5;
            GameField[5, 3] = -5;
            GameField[5, 4] = -5;
            GameField[5, 5] = -5;
            GameField[5, 6] = -5;

            GameField[6, 2] = -5;
            GameField[6, 4] = -5;

            GameField[7, 0] = -5;
            GameField[7, 1] = -5;
            GameField[7, 6] = -5;


            Position pos = player1.king.FindNearestPointCorridorStartegy(player1.Сompetitor.queen, player1.Сompetitor.king);
            Assert.AreEqual(-1, pos.Row);
            Assert.AreEqual(-1, pos.Column);

            int result = player1.king.MoveKingCorridorStartegy(100, player1.posEnd, player1.Сompetitor.queen, player1.Сompetitor.king, player1.history, 1, player1.queen);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода
        /// </summary>
        public void StartegyCorridorTest8()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            player1.king.MoveFigure(2, 7);
            player2.king.MoveFigure(2, 4);
            player1.queen.MoveFigure(1, 2);
            player2.queen.MoveFigure(6, 4);

            GameField[0, 0] = -5;
            GameField[0, 2] = -5;
            GameField[0, 5] = -5;
            GameField[0, 7] = -5;

            GameField[1, 1] = -5;
            GameField[1, 7] = -5;

            GameField[2, 0] = -5;
            GameField[2, 5] = -5;
            GameField[2, 6] = -5;

            GameField[3, 4] = -5;
            GameField[3, 6] = -5;

            GameField[4, 4] = -5;
            GameField[4, 7] = -5;

            GameField[5, 0] = -5;
            GameField[5, 1] = -5;
            GameField[5, 3] = -5;
            GameField[5, 7] = -5;

            GameField[6, 1] = -5;
            GameField[6, 6] = -5;
            GameField[6, 7] = -5;

            GameField[7, 1] = -5;

            bool check = player1.queen.ObstacleOrNearbyMove(player1.Сompetitor.king, 1, player1.history, 6);
            Assert.AreEqual(false, check);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода
        /// </summary>
        public void StartegyCorridorTest9()
        {
            Field GameField = new Field(8, 8);
            Player player1 = new Player(Color.White, ref GameField);
            Player player2 = new Player(Color.Black, ref GameField);
            player1.Сompetitor = player2;
            player2.Сompetitor = player1;

            GameField[0, 5] = -5;

            GameField[1, 4] = -5;

            GameField[2, 4] = -5;
            GameField[2, 5] = -5;

            GameField[3, 7] = -5;

            GameField[4, 0] = -5;
            GameField[4, 2] = -5;
            GameField[4, 3] = -5;
            GameField[4, 4] = -5;
            GameField[4, 6] = -5;

            GameField[5, 1] = -5;
            GameField[5, 2] = -5;
            GameField[5, 4] = -5;
            GameField[5, 5] = -5;
            GameField[5, 6] = -5;
            GameField[5, 7] = -5;

            GameField[6, 1] = -5;
            GameField[6, 7] = -5;

            GameField[7, 0] = -5;
            GameField[7, 5] = -5;
            GameField[7, 6] = -5;

            Position pos = player2.king.FindNearestPointCorridorStartegy(player2.Сompetitor.queen, player2.Сompetitor.king);
            Assert.AreEqual(5, pos.Row);
            Assert.AreEqual(3, pos.Column);

            int result = player2.king.MoveKingCorridorStartegy(100, player2.posEnd, player2.Сompetitor.queen, player2.Сompetitor.king, player2.history, 1, player2.queen);
            Assert.AreEqual(1, result);
        }
    }
}
