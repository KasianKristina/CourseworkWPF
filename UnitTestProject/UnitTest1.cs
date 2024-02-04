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

            int result = DynamicField.getResult(player1);

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

            int result = DynamicField.getResult(player1);

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

            int result = DynamicField.getResult(player1);

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

            int result = DynamicField.getResult(player1);

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

            int result = DynamicField.getResult(player1);

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

            int result = DynamicField.getResult(player1);

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

            int result = DynamicField.minMax(player1, 1, player1.king.Offset, player1.queen.Offset, player2.king.Offset, player2.queen.Offset, int.MinValue, int.MaxValue, 1);

            Assert.AreEqual(-400, result);
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

            int result = DynamicField.minMax(player2, 0, player1.king.Offset, player1.queen.Offset, player2.king.Offset, player2.queen.Offset, int.MinValue, int.MaxValue, 0);

            Assert.AreEqual(-300, result);
        }

        [TestMethod]
        /// <summary>
        /// Тест для проверки метода minMax. Позиция для хода к1 = (6, 5). 4-(-200) = 204
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

            int result = DynamicField.minMax(player1, 1, player1.king.Offset, player1.queen.Offset, player2.king.Offset, player2.queen.Offset, int.MinValue, int.MaxValue, 1);

            Assert.AreEqual(-204, result);
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

            DynamicField.minMax(player1, 1, player1.king.Offset, player1.queen.Offset, player2.king.Offset, player2.queen.Offset, int.MinValue, int.MaxValue, 1);
            Assert.AreEqual(6, player1.king.Offset.Row);
            Assert.AreEqual(5, player1.king.Offset.Column);

             DynamicField.minMax(player2, 1, player1.king.Offset, player1.queen.Offset, player2.king.Offset, player2.queen.Offset, int.MinValue, int.MaxValue, 1);
            Assert.AreEqual(0, player2.queen.Offset.Row);
            Assert.AreEqual(4, player2.queen.Offset.Column);

            DynamicField.minMax(player1, 1, player1.king.Offset, player1.queen.Offset, player2.king.Offset, player2.queen.Offset, int.MinValue, int.MaxValue, 1);
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

            DynamicField.minMax(player1, 2, player1.king.Offset, player1.queen.Offset, player2.king.Offset, player2.queen.Offset, int.MinValue, int.MaxValue, 2);

            Assert.AreEqual(6, player1.king.Offset.Row);
            Assert.AreEqual(5, player1.king.Offset.Column);
        }
    }
}
