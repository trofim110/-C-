using System;
using KbAis.OpenPit.Core;

namespace KbAis.OpenPit.Core
{
    /// <summary>
    /// Основная функция конструктор polyLabel с нее начинается работа программы.
    /// </summary>
    public class PolyLabel
    {
        /// <summary>
        /// Вложенный  класс существует для нахождения параметров разбивочного квадарата.
        /// </summary>
        private class Cell
    {
        /// <summary>
        /// Основная  информация о квадратах(Координаты центра,половина стороны,дистанция от центра до многоугольника и max)
        /// </summary>
        public double x, y, half, distance, max;

        /// <summary>
        /// В конструкторе высчитываем distance дистанцию от центра квадрата до многоугольника и параметр max
        /// <param name="x">Координата Х центра квадрата.</param>
        /// <param name="y">Координата У центра квадрата.</param>
        /// <param name="half">Половина стороны квадрата.</param>
        /// <param name="polygon">Координаты многоугольника
        /// (многоугольников может быть много поэтому мы используем трехмерный масив).</param>
        /// </summary>
        public Cell(double x, double y, double half, double[][][] polygon)
        {
            this.x = x;
            this.y = y;
            this.half = half;
            this.distance = PointToPolygonDist(x, y, polygon);
            this.max = this.distance + this.half * System.Math.Sqrt(2);
            Console.WriteLine($"     {this.x}    {this.y}       {distance}     {max}");
        }
        /// <summary>
        /// Перебирает все стороны многоуг[ольника и выдает минимальную дистанцию от цента ячейки до многоугольника.
        /// Если центр квадрата не лежит в многоугольнике то дистанция отрицательная.
        /// </summary>
        /// <param name="x">Координата Х центра квадрата.</param>
        /// <param name="y">Координата У центра квадрата.</param>
        /// <param name="polygon">Координаты многоугольника.</param>
        /// <returns>
        /// Возврощает минимальное значение расстояния от цента ячейки до многоугольника.
        /// </returns>
        private static double PointToPolygonDist(double x, double y, double[][][] polygon)
        {
            var inside = false;
            // Содержит ли многоугольник центр квадрата.
            var minDistSq = double.MaxValue;

            foreach (var ring in polygon)
                for (var i = 0; i < ring.Length; i++)
                {
                    var current = ring[(i + 1) % ring.Length];
                    //Точка многоугольника I+1(Когда дойдет до конечной точки многоугольника возвращается к первой точке)
                    var prev = ring[i];
                    //Точка многоугольника I

                    inside = FindingTheCenter(x, y, current, prev, inside);

                    var distSq = GetSegDistSq(x, y, current, prev);

                    if (distSq < minDistSq) minDistSq = distSq;
                    //Задает минимальное расстояние
                }
            return minDistSq == 0 ? 0 : (inside ? 1 : -1) * System.Math.Sqrt(minDistSq);
        }
        /// <summary>
        /// Cодержит ли многоугольник центр квадрата.
        /// </summary>
        /// <param name="x"> Координата Х центра квадрата.</param>
        /// <param name="y"> Координата У центра квадрата.</param>
        /// <param name="current"> Кордината вершины многоугольника i+1.</param>
        /// <param name="prev"> Кордината вершины многоугольника i.</param>
        /// <param name="inside"> Содержит ли многоугольник центр квадрата.</param>
        /// <returns> Информация содержит ли многоугольник центр квадрата(True если содержит). </returns>
        private static bool FindingTheCenter(double x, double y, double[] current, double[] prev,bool inside)
        {
            if ((current[1] > y != prev[1] > y)
                &&(x < (prev[0] - current[0]) * (y - current[1]) / (prev[1] - current[1]) + current[0]))
            {
                return !inside;
            }
            return inside;
        }
        /// <summary>
        ///  Минимальное расстояние от центра ячейки до прямой(Образоваными двумя точками, если точнее для стороны многоугольника).
        /// </summary>
        /// <param name="px">Координата Х центра квадрата.</param>
        /// <param name="py">Координата У центра квадрата.</param>
        /// <param name="a"> Координата текущей вершины многоугольника.</param>
        /// <param name="b">Координата предыдущей вершины многоугольника.</param>
        /// <returns>
        /// Возвращает минимальное значение между точкой прямой (без корня!!!!!Это важно! это неполная формулы расстояния между точками ).
        /// </returns>
      private static double GetSegDistSq(double px, double py, double[] a, double[] b)
        {
            var x = a[0];
            var y = a[1];
            var dx = b[0] - x;
            var dy = b[1] - y;
            if (dx != 0 || dy != 0)
            {
                var t = ((px - x) * dx + (py - y) * dy) / (dx * dx + dy * dy);

                if (t > 1)
                {
                    x = b[0];
                    y = b[1];
                }
                else if (t > 0)
                {
                    x += dx * t;
                    y += dy * t;
                }
            }

            dx = px - x;
            dy = py - y;
            return dx * dx + dy * dy;
        }
    }
        /// <summary>
        /// Основной конструктор с которого начинается программа
        /// </summary>
        /// <param name="polygon">Координаты многоугольника</param>
        /// <param name="precision">Точность</param>
        public PolyLabel(double[][][] polygon, double precision)
        {

            var minX = polygon[0][0][0]; //
            var maxX = minX;
            var minY = polygon[0][0][1];
            var maxY = minY;
            for (var i = 1; i < polygon[0].Length; i++)
            {
                var x = polygon[0][i][0];
                var y = polygon[0][i][1];
                if (x < minX) minX = x;
                if (x > maxX) maxX = x;
                if (y < minY) minY = y;
                if (y > maxY) maxY = y;
            }

            var width = maxX - minX;
            var height = maxY - minY;
            // Находит максимальную высоту и ширину многоугольника.
            var cellSize = System.Math.Min(width, height);
            // Задает размер квадрата разбиения (Размер минимальноя сторона многоугольника).
            var half = cellSize / 2.0d;
            //Проверяет чтобы размер многоугольника был не равен нулю.


            if (cellSize == 0)
            {
                throw new Exception($"cellSize=0 x= {minX} y= {minY}  distan=0  ");
            }

            //Создаем колекцию преорететной  очереди cellQueue с типом (класса) Cell
            var cellQueue = new PriorityQueue<Cell, double>();
            // С помощь циклов разбивает многоугольник на квадраты
            for (var x = minX; x < maxX; x += cellSize)
            for (var y = minY; y < maxY; y += cellSize)
                //   Создаем объект класса Cell c даными квадрата
                cellQueue.Enqueue(new Cell(x + half, y + half, half, polygon),
                    new Cell(x + half, y + half, half, polygon).max * (-1));
            //  Все созданные объекты добавляются в очередь cellQueue с приорететом равным значение max *(-1)

            //Создаем  Cell bestCell - (возьмем центроид в качестве первого наилучшего предположения)
            var bestCell = GetCentroidCell(polygon);
            //особый случай для прямоугольных многоугольников
            var bboxCell = new Cell(minX + width / 2, minY + height / 2, 0, polygon);

            if (bboxCell.distance > bestCell.distance)
                bestCell = bboxCell;


            //Создаем numProbes-количество обектов в очереди cellQueue
            var numProbes = cellQueue.Count;

            while (cellQueue.Count > 0)
            {
                //Из очереди  cellQueue с помощью метода Dequeue удаляет объект c наименьшим приорететом   и возвращает его в  Cell cell.
                var cell = cellQueue.Dequeue();
                if (cell.distance > bestCell.distance)
                {
                    bestCell = cell;
                }

                //  не углубляйтесь дальше, если нет никаких шансов на лучшее решение
                if (cell.max - bestCell.distance <= precision)
                    continue;
                //  разделите ячейку на четыре ячейки
                half = cell.half / 2.0d;
                cellQueue.Enqueue(new Cell(cell.x - half, cell.y - half, half, polygon),
                    new Cell(cell.x - half, cell.y - half, half, polygon).max * (-1));
                cellQueue.Enqueue(new Cell(cell.x + half, cell.y - half, half, polygon),
                    new Cell(cell.x + half, cell.y - half, half, polygon).max * (-1));
                cellQueue.Enqueue(new Cell(cell.x - half, cell.y + half, half, polygon),
                    new Cell(cell.x - half, cell.y + half, half, polygon).max * (-1));
                cellQueue.Enqueue(new Cell(cell.x + half, cell.y + half, half, polygon),
                    new Cell(cell.x + half, cell.y + half, half, polygon).max * (-1));

                numProbes += 4;
            }

            this.x = bestCell.x;
            this.y = bestCell.y;
            this.distance = bestCell.distance;
        }

        private void Add()
        {

        }
        /// <summary>
        /// Метод находит центроид по координатам многоугольника.
        /// </summary>
        /// <param name="polygon"> Координаты многоугольника.</param>
        /// <returns>
        /// Возвращает центроид многоугольника в  обекты класса Cell.
        /// </returns>
        private static Cell GetCentroidCell(double[][][] polygon)
        {
            double area = 0;
            double x = 0;
            double y = 0;
            var points = polygon[0];

            for (int i = 0, len = points.Length, j = len - 1; i < len; j = i++)
            {
                var a0 = points[i][0];
                var a1 = points[i][1];
                var b0 = points[j][0];
                var b1 = points[j][1];
                var diff = a0 * b1 - b0 * a1;
                x += (a0 + b0) * diff;
                y += (a1 + b1) * diff;
                area += diff * 3;
            }

            if (area == 0)
                return new Cell(points[0][0], points[0][1], 0, polygon);
            return new Cell(x / area, y / area, 0, polygon);
        }

        public double x { get; set; }
        public double y { get; set; }
        public double distance { get; set; }
    }
}