
namespace FormulaD_Logic.Data.Tracks {
    public class Monaco : Track {
        public Monaco() {
            AddGridChain();
            AddGrid();
            GenerateStandardDice();
            ConvertTurnsToStraightDirection();
        }

        private void AddGrid() {
            // Straight 1
            GenerateGrid(1, 21, false, 0);
            GenerateGrid(169, 190, false, 0);
            GenerateGrid(335, 357, false, 0);

            // Turn 1
            GenerateGrid(22, 28, true, 1);
            GenerateGrid(191, 195, true, 1);
            GenerateGrid(358, 360, true, 1);

            // Straight 2
            GenerateGrid(29, 46, false, 0);
            GenerateGrid(196, 213, false, 0);
            GenerateGrid(361, 378, false, 0);

            // Turn 2
            GenerateGrid(47, 51, true, 1);
            GenerateGrid(214, 221, true, 1);
            GenerateGrid(379, 388, true, 1);

            // Straight 3
            GenerateGrid(52, 53, false, 0);
            GenerateGrid(222, 223, false, 0);
            GenerateGrid(389, 390, false, 0);

            // Turn 3
            GenerateGrid(54, 59, true, 1);
            GenerateGrid(224, 227, true, 1);
            GenerateGrid(391, 392, true, 1);

            // Straight 4
            GenerateGrid(60, 66, false, 0);
            GenerateGrid(228, 235, false, 0);
            GenerateGrid(393, 401, false, 0);

            // Turn 4
            GenerateGrid(67, 72, true, 1);
            GenerateGrid(236, 239, true, 1);
            GenerateGrid(402, 403, true, 1);

            // Straight 5
            GenerateGrid(73, 75, false, 0);
            GenerateGrid(240, 242, false, 0);
            GenerateGrid(404, 406, false, 0);

            // Turn 5
            GenerateGrid(76, 87, true, 3);
            GenerateGrid(243, 254, true, 3);
            GenerateGrid(407, 418, true, 3);

            // Straight 6
            GenerateGrid(88, 115, false, 0);
            GenerateGrid(255, 282, false, 0);
            GenerateGrid(419, 446, false, 0);

            // Turn 6
            GenerateGrid(116, 120, true, 1);
            GenerateGrid(283, 288, true, 1);
            GenerateGrid(447, 453, true, 1);

            // Straight 7
            GenerateGrid(121, 129, false, 0);
            GenerateGrid(289, 296, false, 0);
            GenerateGrid(454, 460, false, 0);

            // Turn 7
            GenerateGrid(130, 132, true, 1);
            GenerateGrid(297, 301, true, 1);
            GenerateGrid(461, 467, true, 1);

            // Straight 8
            GenerateGrid(133, 135, false, 0);
            GenerateGrid(302, 303, false, 0);
            GenerateGrid(468, 468, false, 0);

            // Turn 8
            GenerateGrid(136, 144, true, 2);
            GenerateGrid(304, 313, true, 2);
            GenerateGrid(469, 479, true, 2);

            // Straight 9
            GenerateGrid(145, 148, false, 0);
            GenerateGrid(314, 316, false, 0);
            GenerateGrid(480, 481, false, 0);

            // Turn 9
            GenerateGrid(149, 157, true, 2);
            GenerateGrid(317, 324, true, 2);
            GenerateGrid(482, 488, true, 2);

            // Straight 10
            GenerateGrid(158, 162, false, 0);
            GenerateGrid(325, 330, false, 0);
            GenerateGrid(489, 495, false, 0);

            // Turn 10
            GenerateGrid(163, 168, true, 1);
            GenerateGrid(331, 334, true, 1);
            GenerateGrid(496, 497, true, 1);

            // Start
            Grid.GetBySpotNumber(4).IsStart = true;
            Grid.GetBySpotNumber(7).IsStart = true;
            Grid.GetBySpotNumber(10).IsStart = true;
            Grid.GetBySpotNumber(13).IsStart = true;
            Grid.GetBySpotNumber(16).IsStart = true;
            Grid.GetBySpotNumber(338).IsStart = true;
            Grid.GetBySpotNumber(341).IsStart = true;
            Grid.GetBySpotNumber(344).IsStart = true;
            Grid.GetBySpotNumber(347).IsStart = true;
            Grid.GetBySpotNumber(350).IsStart = true;
            Grid.GetBySpotNumber(17).IsFinish = true;
            Grid.GetBySpotNumber(186).IsFinish = true;
            Grid.GetBySpotNumber(352).IsFinish = true;
        }

        private void AddGridChain() {

            // Straight All
            GenerateChain(1, 167, 2, GridChain.enumDirectionType.Straight);
            GenerateChain(169, 333, 170, GridChain.enumDirectionType.Straight);
            GenerateChain(335, 496, 336, GridChain.enumDirectionType.Straight);

            // Inward from outside
            GenerateChain(1, 24, 170, GridChain.enumDirectionType.Inward);
            GenerateChain(26, 45, 194, GridChain.enumDirectionType.Inward);
            GenerateChain(52, 56, 222, GridChain.enumDirectionType.Inward);
            GenerateChain(58, 69, 227, GridChain.enumDirectionType.Inward);
            GenerateChain(71, 74, 239, GridChain.enumDirectionType.Inward);
            GenerateChain(77, 79, 247, GridChain.enumDirectionType.Inward);
            GenerateChain(81, 84, 250, GridChain.enumDirectionType.Inward);
            GenerateChain(86, 114, 254, GridChain.enumDirectionType.Inward);
            GenerateChain(117, 117, 286, GridChain.enumDirectionType.Inward);
            GenerateChain(119, 128, 287, GridChain.enumDirectionType.Inward);
            GenerateChain(133, 134, 302, GridChain.enumDirectionType.Inward);
            GenerateChain(136, 136, 306, GridChain.enumDirectionType.Inward);
            GenerateChain(138, 141, 307, GridChain.enumDirectionType.Inward);
            GenerateChain(145, 147, 314, GridChain.enumDirectionType.Inward);
            GenerateChain(149, 150, 319, GridChain.enumDirectionType.Inward);
            GenerateChain(152, 153, 321, GridChain.enumDirectionType.Inward);
            GenerateChain(155, 164, 323, GridChain.enumDirectionType.Inward);
            GenerateChain(166, 167, 333, GridChain.enumDirectionType.Inward);

            // Inward from middle
            GenerateChain(169, 189, 336, GridChain.enumDirectionType.Inward);
            GenerateChain(191, 192, 358, GridChain.enumDirectionType.Inward);
            GenerateChain(194, 212, 360, GridChain.enumDirectionType.Inward);
            GenerateChain(222, 222, 289, GridChain.enumDirectionType.Inward);
            GenerateChain(224, 225, 391, GridChain.enumDirectionType.Inward);
            GenerateChain(227, 234, 393, GridChain.enumDirectionType.Inward);
            GenerateChain(236, 236, 402, GridChain.enumDirectionType.Inward);
            GenerateChain(238, 241, 403, GridChain.enumDirectionType.Inward);
            GenerateChain(247, 248, 414, GridChain.enumDirectionType.Inward);
            GenerateChain(250, 252, 416, GridChain.enumDirectionType.Inward);
            GenerateChain(254, 281, 419, GridChain.enumDirectionType.Inward);
            GenerateChain(286, 287, 451, GridChain.enumDirectionType.Inward);
            GenerateChain(289, 295, 454, GridChain.enumDirectionType.Inward);
            GenerateChain(302, 302, 468, GridChain.enumDirectionType.Inward);
            GenerateChain(305, 305, 472, GridChain.enumDirectionType.Inward);
            GenerateChain(307, 309, 473, GridChain.enumDirectionType.Inward);
            GenerateChain(314, 315, 480, GridChain.enumDirectionType.Inward);
            GenerateChain(320, 321, 486, GridChain.enumDirectionType.Inward);
            GenerateChain(323, 329, 488, GridChain.enumDirectionType.Inward);
            GenerateChain(331, 331, 496, GridChain.enumDirectionType.Inward);
            GenerateChain(333, 333, 497, GridChain.enumDirectionType.Inward);

            // Outward from inside
            GenerateChain(335, 356, 169, GridChain.enumDirectionType.Outward);
            GenerateChain(361, 382, 196, GridChain.enumDirectionType.Outward);
            GenerateChain(384, 389, 218, GridChain.enumDirectionType.Outward);
            GenerateChain(393, 400, 228, GridChain.enumDirectionType.Outward);
            GenerateChain(404, 408, 240, GridChain.enumDirectionType.Outward);
            GenerateChain(410, 410, 245, GridChain.enumDirectionType.Outward);
            GenerateChain(412, 413, 246, GridChain.enumDirectionType.Outward);
            GenerateChain(419, 448, 255, GridChain.enumDirectionType.Outward);
            GenerateChain(452, 463, 288, GridChain.enumDirectionType.Outward);
            GenerateChain(465, 470, 300, GridChain.enumDirectionType.Outward);
            GenerateChain(475, 475, 311, GridChain.enumDirectionType.Outward);
            GenerateChain(477, 483, 312, GridChain.enumDirectionType.Outward);
            GenerateChain(489, 494, 325, GridChain.enumDirectionType.Outward);

            // Outward from middle
            GenerateChain(169, 189, 1, GridChain.enumDirectionType.Outward);
            GenerateChain(196, 212, 29, GridChain.enumDirectionType.Outward);
            GenerateChain(214, 214, 47, GridChain.enumDirectionType.Outward);
            GenerateChain(216, 218, 48, GridChain.enumDirectionType.Outward);
            GenerateChain(220, 222, 51, GridChain.enumDirectionType.Outward);
            GenerateChain(228, 234, 60, GridChain.enumDirectionType.Outward);
            GenerateChain(240, 241, 73, GridChain.enumDirectionType.Outward);
            GenerateChain(243, 243, 76, GridChain.enumDirectionType.Outward);
            GenerateChain(246, 246, 77, GridChain.enumDirectionType.Outward);
            GenerateChain(255, 281, 88, GridChain.enumDirectionType.Outward);
            GenerateChain(283, 283, 116, GridChain.enumDirectionType.Outward);
            GenerateChain(288, 295, 121, GridChain.enumDirectionType.Outward);
            GenerateChain(297, 298, 130, GridChain.enumDirectionType.Outward);
            GenerateChain(300, 302, 132, GridChain.enumDirectionType.Outward);
            GenerateChain(304, 304, 136, GridChain.enumDirectionType.Outward);
            GenerateChain(310, 310, 143, GridChain.enumDirectionType.Outward);
            GenerateChain(312, 315, 144, GridChain.enumDirectionType.Outward);
            GenerateChain(317, 318, 149, GridChain.enumDirectionType.Outward);
            GenerateChain(325, 329, 158, GridChain.enumDirectionType.Outward);

            // Connect last squares to first
            Chain.AddGridChain(new GridChain() {
                Direction = GridChain.enumDirectionType.Straight,
                StopNumberFrom = 168,
                StopNumberTo = 1
            });
            Chain.AddGridChain(new GridChain() {
                Direction = GridChain.enumDirectionType.Inward,
                StopNumberFrom = 168,
                StopNumberTo = 169
            });
            Chain.AddGridChain(new GridChain() {
                Direction = GridChain.enumDirectionType.Straight,
                StopNumberFrom = 334,
                StopNumberTo = 168
            });
            Chain.AddGridChain(new GridChain() {
                Direction = GridChain.enumDirectionType.Inward,
                StopNumberFrom = 334,
                StopNumberTo = 335
            });
            Chain.AddGridChain(new GridChain() {
                Direction = GridChain.enumDirectionType.Straight,
                StopNumberFrom = 497,
                StopNumberTo = 335
            });
        }
    }
}
