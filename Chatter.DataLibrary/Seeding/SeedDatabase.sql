﻿DELETE FROM [dbo].[Users];
DELETE FROM [dbo].[Friends];

SET IDENTITY_INSERT [dbo].[Users] ON
INSERT INTO [dbo].[Users] ([Id], [Username]) VALUES
    (1, 'slampett0'),
    (2, 'dcoultish1'),
    (3, 'rloins2'),
    (4, 'alowmass3'),
    (5, 'rpiche4'),
    (6, 'ademschke5'),
    (7, 'cvel6'),
    (8, 'dkraft7'),
    (9, 'cmilbourne8'),
    (10, 'rbellerby9'),
    (11, 'lkornalika'),
    (12, 'lgooseb'),
    (13, 'llummusc'),
    (14, 'asivierd'),
    (15, 'ggowdye'),
    (16, 'dmatthewesf'),
    (17, 'wkilmisterg'),
    (18, 'lbrattellh'),
    (19, 'lwillatti'),
    (20, 'mstuckesj'),
    (21, 'rwhyattk'),
    (22, 'dreddyhoffl'),
    (23, 'hlemmerm'),
    (24, 'rtindalln'),
    (25, 'ggaffono'),
    (26, 'gdearsleyp'),
    (27, 'rferrierioq'),
    (28, 'hbosenworthr'),
    (29, 'rgelardis'),
    (30, 'llyokhint'),
    (31, 'phughlinu'),
    (32, 'jbuggv'),
    (33, 'ojanssensw'),
    (34, 'arosenshinex'),
    (35, 'vmeliory'),
    (36, 'mjackez'),
    (37, 'nvanhove10'),
    (38, 'rsetford11'),
    (39, 'jjohantges12'),
    (40, 'rdoherty13'),
    (41, 'gconnock14'),
    (42, 'fdunmore15'),
    (43, 'emableson16'),
    (44, 'anevill17'),
    (45, 'afrigot18'),
    (46, 'erock19'),
    (47, 'awethered1a'),
    (48, 'rjansky1b'),
    (49, 'naymeric1c'),
    (50, 'ufeldhammer1d'),
    (51, 'astopforth1e'),
    (52, 'cfroschauer1f'),
    (53, 'mgouge1g'),
    (54, 'scousans1h'),
    (55, 'tsimmons1i'),
    (56, 'deliff1j'),
    (57, 'akornalik1k'),
    (58, 'wdeners1l'),
    (59, 'flosbie1m'),
    (60, 'soxenbury1n'),
    (61, 'jbalam1o'),
    (62, 'abrickdale1p'),
    (63, 'bgwalter1q'),
    (64, 'jseyffert1r'),
    (65, 'lglasscott1s'),
    (66, 'mfilipov1t'),
    (67, 'rlemar1u'),
    (68, 'cpavy1v'),
    (69, 'lpetti1w'),
    (70, 'ihector1x'),
    (71, 'clonghirst1y'),
    (72, 'rbamb1z'),
    (73, 'mrenzini20'),
    (74, 'sbentinck21'),
    (75, 'narchley22'),
    (76, 'jfacher23'),
    (77, 'ldupre24'),
    (78, 'dbaudesson25'),
    (79, 'nbowshire26'),
    (80, 'kendean27'),
    (81, 'vmcelroy28'),
    (82, 'svowels29'),
    (83, 'jdarlison2a'),
    (84, 'rfley2b'),
    (85, 'nlavin2c'),
    (86, 'tlorenzini2d'),
    (87, 'wvose2e'),
    (88, 'hwhitter2f'),
    (89, 'ehalleybone2g'),
    (90, 'tgorrick2h'),
    (91, 'gkendell2i'),
    (92, 'fseak2j'),
    (93, 'mbrookton2k'),
    (94, 'lollivierre2l'),
    (95, 'acollopy2m'),
    (96, 'coxterby2n'),
    (97, 'mpawlick2o'),
    (98, 'edehooge2p'),
    (99, 'ltopley2q'),
    (100, 'mgrattage2r'),
    (101, 'pmowday2s'),
    (102, 'clangthorne2t'),
    (103, 'lhasnney2u'),
    (104, 'gbladesmith2v'),
    (105, 'pbevans2w'),
    (106, 'tnovello2x'),
    (107, 'akeyzman2y'),
    (108, 'cclemensen2z'),
    (109, 'aphythien30'),
    (110, 'hconradie31'),
    (111, 'skerner32'),
    (112, 'lbalderston33'),
    (113, 'mduferie34'),
    (114, 'kmusla35'),
    (115, 'npaddell36'),
    (116, 'lcoomer37'),
    (117, 'jskate38'),
    (118, 'sattersoll39'),
    (119, 'kshottin3a'),
    (120, 'teckhard3b'),
    (121, 'vdorricott3c'),
    (122, 'espelwood3d'),
    (123, 'etunbridge3e'),
    (124, 'nrickett3f'),
    (125, 'ctrighton3g'),
    (126, 'mlantiffe3h'),
    (127, 'nsafe3i'),
    (128, 'ballchin3j'),
    (129, 'vspoerl3k'),
    (130, 'rwike3l'),
    (131, 'balvarado3m'),
    (132, 'jbailles3n'),
    (133, 'kmclafferty3o'),
    (134, 'mpeschet3p'),
    (135, 'ewhetnell3q'),
    (136, 'fkillough3r'),
    (137, 'oocorr3s'),
    (138, 'mpryer3t'),
    (139, 'ckayne3u'),
    (140, 'gmordin3v'),
    (141, 'npowlesland3w'),
    (142, 'jmccafferky3x'),
    (143, 'freinhart3y'),
    (144, 'oblazek3z'),
    (145, 'tmangenet40'),
    (146, 'dtrime41'),
    (147, 'afayre42'),
    (148, 'gdoak43'),
    (149, 'oburdess44'),
    (150, 'cplunket45'),
    (151, 'achicchelli46'),
    (152, 'wcaldayrou47'),
    (153, 'kfilmer48'),
    (154, 'rzarfat49'),
    (155, 'mcharlesworth4a'),
    (156, 'brodman4b'),
    (157, 'kvenart4c'),
    (158, 'wigo4d'),
    (159, 'allewellin4e'),
    (160, 'lpowys4f'),
    (161, 'cfreeman4g'),
    (162, 'mhurran4h'),
    (163, 'abartolomieu4i'),
    (164, 'equaif4j'),
    (165, 'mpetry4k'),
    (166, 'lcumbers4l'),
    (167, 'rglossup4m'),
    (168, 'dlindstrom4n'),
    (169, 'squarrington4o'),
    (170, 'gkeoghane4p'),
    (171, 'ebechley4q'),
    (172, 'mtreker4r'),
    (173, 'ccasiero4s'),
    (174, 'ccella4t'),
    (175, 'bbeswetherick4u'),
    (176, 'lsharrier4v'),
    (177, 'bspellar4w'),
    (178, 'jdinesen4x'),
    (179, 'tferrieri4y'),
    (180, 'ngoddman4z'),
    (181, 'lgilderoy50'),
    (182, 'srathborne51'),
    (183, 'pliff52'),
    (184, 'agrassick53'),
    (185, 'ncoppens54'),
    (186, 'radamec55'),
    (187, 'cpepall56'),
    (188, 'lbarras57'),
    (189, 'byannoni58'),
    (190, 'edragonette59'),
    (191, 'fcraven5a'),
    (192, 'hrosenbaum5b'),
    (193, 'frosle5c'),
    (194, 'ascowen5d'),
    (195, 'tbolin5e'),
    (196, 'yfairney5f'),
    (197, 'lcasolla5g'),
    (198, 'sstud5h'),
    (199, 'rsutcliffe5i'),
    (200, 'segginson5j'),
    (201, 'msore5k'),
    (202, 'jmollindinia5l'),
    (203, 'bpetegree5m'),
    (204, 'faudiss5n'),
    (205, 'bfolbig5o'),
    (206, 'shing5p'),
    (207, 'ocharlesworth5q'),
    (208, 'dtrahar5r'),
    (209, 'lfairebrother5s'),
    (210, 'ncrichmere5t'),
    (211, 'kgaler5u'),
    (212, 'npuddin5v'),
    (213, 'rliverock5w'),
    (214, 'dsessuns5x'),
    (215, 'aainslee5y'),
    (216, 'wdoy5z'),
    (217, 'gpratty60'),
    (218, 'etollemache61'),
    (219, 'kbebb62'),
    (220, 'jthom63'),
    (221, 'lmacilwrick64'),
    (222, 'pjaniszewski65'),
    (223, 'kmoorey66'),
    (224, 'jderoeck67'),
    (225, 'kforten68'),
    (226, 'mscamwell69'),
    (227, 'gbennis6a'),
    (228, 'gswinerd6b'),
    (229, 'tjarrard6c'),
    (230, 'cbold6d'),
    (231, 'gglanton6e'),
    (232, 'osherebrooke6f'),
    (233, 'kpeddar6g'),
    (234, 'bjeandillou6h'),
    (235, 'zclement6i'),
    (236, 'chartzogs6j'),
    (237, 'bkibbee6k'),
    (238, 'mfrancescone6l'),
    (239, 'cpaterno6m'),
    (240, 'bhosten6n'),
    (241, 'mdarington6o'),
    (242, 'sallsepp6p'),
    (243, 'mcadany6q'),
    (244, 'kslyme6r'),
    (245, 'rbonde6s'),
    (246, 'ccardis6t'),
    (247, 'ctipping6u'),
    (248, 'slotherington6v'),
    (249, 'srishworth6w'),
    (250, 'bphil6x'),
    (251, 'jcowterd6y'),
    (252, 'alorence6z'),
    (253, 'claverock70'),
    (254, 'doroan71'),
    (255, 'njansky72'),
    (256, 'smoring73'),
    (257, 'mnanuccioi74'),
    (258, 'jdalgleish75'),
    (259, 'nmakeswell76'),
    (260, 'zbartle77'),
    (261, 'cdanbrook78'),
    (262, 'rferrillo79'),
    (263, 'chowen7a'),
    (264, 'swickendon7b'),
    (265, 'ngrinaway7c'),
    (266, 'lshelliday7d'),
    (267, 'gsarton7e'),
    (268, 'bhead7f'),
    (269, 'dgrindall7g'),
    (270, 'loshaughnessy7h'),
    (271, 'mblay7i'),
    (272, 'jlouche7j'),
    (273, 'rbatterton7k'),
    (274, 'kmetts7l'),
    (275, 'ablabie7m'),
    (276, 'jnowakowska7n'),
    (277, 'cgillham7o'),
    (278, 'bbannell7p'),
    (279, 'kthurlbourne7q'),
    (280, 'kcreenan7r'),
    (281, 'lgodspeede7s'),
    (282, 'vbakewell7t'),
    (283, 'bcantera7u'),
    (284, 'tjaqueme7v'),
    (285, 'nrate7w'),
    (286, 'nhartopp7x'),
    (287, 'mstrank7y'),
    (288, 'djelly7z'),
    (289, 'cboddice80'),
    (290, 'mmalletratt81'),
    (291, 'esugarman82'),
    (292, 'tpenson83'),
    (293, 'stillett84'),
    (294, 'fmatzaitis85'),
    (295, 'abutterley86'),
    (296, 'bbraidwood87'),
    (297, 'cesplin88'),
    (298, 'mschooling89'),
    (299, 'vdaunter8a'),
    (300, 'cmcileen8b'),
    (301, 'cbroz8c'),
    (302, 'ekrojn8d'),
    (303, 'fmcgillicuddy8e'),
    (304, 'cgniewosz8f'),
    (305, 'bludvigsen8g'),
    (306, 'ggrimsdyke8h'),
    (307, 'nbatchelar8i'),
    (308, 'mdudden8j'),
    (309, 'mgrealy8k'),
    (310, 'swhitloe8l'),
    (311, 'cbalden8m'),
    (312, 'osouthorn8n'),
    (313, 'kskipsey8o'),
    (314, 'dosgarby8p'),
    (315, 'aporrett8q'),
    (316, 'mgeelan8r'),
    (317, 'tchittey8s'),
    (318, 'tlesieur8t'),
    (319, 'creneke8u'),
    (320, 'hmation8v'),
    (321, 'htrays8w'),
    (322, 'mdovydenas8x'),
    (323, 'cmeighan8y'),
    (324, 'sjones8z'),
    (325, 'jgorrissen90'),
    (326, 'lmcglaughn91'),
    (327, 'joffin92'),
    (328, 'avahl93'),
    (329, 'emullins94'),
    (330, 'rstannering95'),
    (331, 'dmcvicker96'),
    (332, 'iblumfield97'),
    (333, 'pkelemen98'),
    (334, 'valdritt99'),
    (335, 'osampson9a'),
    (336, 'ftregidgo9b'),
    (337, 'asalvidge9c'),
    (338, 'owhelpton9d'),
    (339, 'brobjohns9e'),
    (340, 'nbrownsall9f'),
    (341, 'sduddell9g'),
    (342, 'wpierton9h'),
    (343, 'rkordes9i'),
    (344, 'zheskin9j'),
    (345, 'rbruffell9k'),
    (346, 'smaken9l'),
    (347, 'ksworder9m'),
    (348, 'zkearton9n'),
    (349, 'kstevenson9o'),
    (350, 'vdavie9p'),
    (351, 'apatman9q'),
    (352, 'srumble9r'),
    (353, 'cjahn9s'),
    (354, 'ebaltzar9t'),
    (355, 'mhobbert9u'),
    (356, 'asantori9v'),
    (357, 'gwolstenholme9w'),
    (358, 'cgilliland9x'),
    (359, 'zgilchrist9y'),
    (360, 'stussaine9z'),
    (361, 'twighta0'),
    (362, 'cfeeherya1'),
    (363, 'whubblea2'),
    (364, 'ncrocombea3'),
    (365, 'mspriggina4'),
    (366, 'kpyffea5'),
    (367, 'kdawnaya6'),
    (368, 'mfranciottia7'),
    (369, 'wtimlina8'),
    (370, 'gclampetta9'),
    (371, 'esellanaa'),
    (372, 'ajorczykab'),
    (373, 'ahardstaffac'),
    (374, 'amcglaughnad'),
    (375, 'dburrowsae'),
    (376, 'fwolveyaf'),
    (377, 'sfilochovag'),
    (378, 'vmcewanah'),
    (379, 'plerwellai'),
    (380, 'dbutteaj'),
    (381, 'hcomussoak'),
    (382, 'ginsollal'),
    (383, 'mhallardam'),
    (384, 'etwiggeran'),
    (385, 'mkloskaao'),
    (386, 'amcmarquisap'),
    (387, 'adorranceaq'),
    (388, 'lferbrachear'),
    (389, 'mleithharveyas'),
    (390, 'fjanssensat'),
    (391, 'kninehamau'),
    (392, 'cmaiklemav'),
    (393, 'mhoudmontaw'),
    (394, 'dharropax'),
    (395, 'cputnamay'),
    (396, 'fcossoraz'),
    (397, 'jgoodluckb0'),
    (398, 'blamontb1'),
    (399, 'pyouhillb2'),
    (400, 'rmcquinb3'),
    (401, 'gcottageb4'),
    (402, 'ygaughanb5'),
    (403, 'emathivatb6'),
    (404, 'kcaustickb7'),
    (405, 'dmathersonb8'),
    (406, 'adupreyb9'),
    (407, 'nbamforthba'),
    (408, 'fjepsonbb'),
    (409, 'sboykettbc'),
    (410, 'koliphandbd'),
    (411, 'hgyorgybe'),
    (412, 'fsodorbf'),
    (413, 'cardenbg'),
    (414, 'rwellandbh'),
    (415, 'ashipcottbi'),
    (416, 'cwalentynowiczbj'),
    (417, 'owhelanbk'),
    (418, 'nlewcockbl'),
    (419, 'mmortellbm'),
    (420, 'blackeybn'),
    (421, 'knisardbo'),
    (422, 'smatuszkiewiczbp'),
    (423, 'ldecavillebq'),
    (424, 'mpaulssonbr'),
    (425, 'sstillerbs'),
    (426, 'klakendenbt'),
    (427, 'awardleybu'),
    (428, 'egoodladbv'),
    (429, 'dwitherowbw'),
    (430, 'glagebx'),
    (431, 'kburrisby'),
    (432, 'edicarlobz'),
    (433, 'ahastlerc0'),
    (434, 'bhandc1'),
    (435, 'rlundbechc2'),
    (436, 'tmicahc3'),
    (437, 'bhallickc4'),
    (438, 'hreveningc5'),
    (439, 'awalcotc6'),
    (440, 'cboundc7'),
    (441, 'kletchfordc8'),
    (442, 'gmauvinc9'),
    (443, 'bmingetca'),
    (444, 'dbosomworthcb'),
    (445, 'akillfordcc'),
    (446, 'mcossentinecd'),
    (447, 'fgregolce'),
    (448, 'dpennockcf'),
    (449, 'ascocroftcg'),
    (450, 'vcharkech'),
    (451, 'sbarthelmeci'),
    (452, 'iartheycj'),
    (453, 'gmacclureck'),
    (454, 'ghosburncl'),
    (455, 'aoliverascm'),
    (456, 'mcritchlowcn'),
    (457, 'epolglaseco'),
    (458, 'sivamycp'),
    (459, 'pandreoucq'),
    (460, 'kmacneicecr'),
    (461, 'cskittrellcs'),
    (462, 'ntittletrossct'),
    (463, 'aphinncu'),
    (464, 'zhemphallcv'),
    (465, 'oasberycw'),
    (466, 'jpoysercx'),
    (467, 'crushforthcy'),
    (468, 'tdrehercz'),
    (469, 'cbonhommed0'),
    (470, 'jfilderyd1'),
    (471, 'sknapmand2'),
    (472, 'stenpennyd3'),
    (473, 'mgarnsworthd4'),
    (474, 'kscorahd5'),
    (475, 'mcalrowd6'),
    (476, 'lsethd7'),
    (477, 'amarrisond8'),
    (478, 'jmcclancyd9'),
    (479, 'emickanda'),
    (480, 'rlydalldb'),
    (481, 'opearsondc'),
    (482, 'dfrieddd'),
    (483, 'abrastedde'),
    (484, 'kayarsdf'),
    (485, 'dbarkerdg'),
    (486, 'fscapelhorndh'),
    (487, 'agilardidi'),
    (488, 'tshallcrossdj'),
    (489, 'memertondk'),
    (490, 'bdumbralldl'),
    (491, 'cwalduckdm'),
    (492, 'ctothedn'),
    (493, 'plearmouthdo'),
    (494, 'boldfielddp'),
    (495, 'ccoxalldq'),
    (496, 'chanbridgedr'),
    (497, 'ccumesds'),
    (498, 'ejellybranddt'),
    (499, 'lnichollsdu'),
    (500, 'pyakobdv'),
    (501, 'fbaroschdw'),
    (502, 'koswickdx'),
    (503, 'alagneauxdy'),
    (504, 'alambisdz'),
    (505, 'bcocozzae0'),
    (506, 'afayee1'),
    (507, 'tmaffeoe2'),
    (508, 'jsemechike3'),
    (509, 'jhedgemane4'),
    (510, 'osiviore5'),
    (511, 'adalemane6'),
    (512, 'gspicke7'),
    (513, 'wgrundwatere8'),
    (514, 'pfisbye9'),
    (515, 'odictyea'),
    (516, 'jcoileyeb'),
    (517, 'dkoeneec'),
    (518, 'mpretoriused'),
    (519, 'vmcquieee'),
    (520, 'faxonef'),
    (521, 'gmatteoeg'),
    (522, 'poneileh'),
    (523, 'sbardeei'),
    (524, 'bstucksburyej'),
    (525, 'rdurrantek'),
    (526, 'fvelel'),
    (527, 'aearnshawem'),
    (528, 'svainen'),
    (529, 'aoquineo'),
    (530, 'drannep'),
    (531, 'flumsdoneq'),
    (532, 'ishellsheereer'),
    (533, 'roaklyes'),
    (534, 'mbleadenet'),
    (535, 'nmacnucatoreu'),
    (536, 'ahurleyev'),
    (537, 'dtrahearew'),
    (538, 'bmordecaiex'),
    (539, 'adulingey'),
    (540, 'kparkhouseez'),
    (541, 'spedlowf0'),
    (542, 'kjuhrukef1'),
    (543, 'msabanf2'),
    (544, 'hguidof3'),
    (545, 'tsandbatchf4'),
    (546, 'twateridgef5'),
    (547, 'rmalcolmsonf6'),
    (548, 'rpietraszekf7'),
    (549, 'gdowryf8'),
    (550, 'clucyf9'),
    (551, 'adulwitchfa'),
    (552, 'braglessfb'),
    (553, 'mcolloughfc'),
    (554, 'csmalemanfd'),
    (555, 'rmilroyfe'),
    (556, 'tbauckhamff'),
    (557, 'abaumerfg'),
    (558, 'hcarsbergfh'),
    (559, 'hconeybeerfi'),
    (560, 'fbessettfj'),
    (561, 'jmellorfk'),
    (562, 'ssarginsonfl'),
    (563, 'bgyppesfm'),
    (564, 'abyshfn'),
    (565, 'kemanulssonfo'),
    (566, 'dryalfp'),
    (567, 'bpidgeleyfq'),
    (568, 'abaudinetfr'),
    (569, 'cbunclarkfs'),
    (570, 'vcranshawft'),
    (571, 'fdepperfu'),
    (572, 'smanfordfv'),
    (573, 'eneilanfw'),
    (574, 'lscoughfx'),
    (575, 'smeehanfy'),
    (576, 'mparadesfz'),
    (577, 'atonryg0'),
    (578, 'cskulletg1'),
    (579, 'ranticg2'),
    (580, 'gdriesg3'),
    (581, 'lcardg4'),
    (582, 'bsemang5'),
    (583, 'nkibardg6'),
    (584, 'cvanderkruysg7'),
    (585, 'avalderg8'),
    (586, 'nwolfeg9'),
    (587, 'acapronga'),
    (588, 'slicciardogb'),
    (589, 'blightfootgc'),
    (590, 'preddyhoffgd'),
    (591, 'kstovingge'),
    (592, 'dwhittingtongf'),
    (593, 'seltunegg'),
    (594, 'chartfieldgh'),
    (595, 'lfineygi'),
    (596, 'mlambertsgj'),
    (597, 'cfowkesgk'),
    (598, 'kambroisegl'),
    (599, 'ptemplemangm'),
    (600, 'ngallamoregn'),
    (601, 'mwilborgo'),
    (602, 'akaplingp'),
    (603, 'mpalethorpegq'),
    (604, 'ohamlettgr'),
    (605, 'lballingergs'),
    (606, 'lfoggogt'),
    (607, 'cthumanngu'),
    (608, 'kyouensgv'),
    (609, 'imattschasgw'),
    (610, 'ckieragx'),
    (611, 'aburnallgy'),
    (612, 'ohaselgrovegz'),
    (613, 'psainthillh0'),
    (614, 'ivanveldeh1'),
    (615, 'ktireh2'),
    (616, 'tbremenh3'),
    (617, 'mluchellih4'),
    (618, 'jcrowsonh5'),
    (619, 'cmcilrathh6'),
    (620, 'skermith7'),
    (621, 'oglennieh8'),
    (622, 'aboiseh9'),
    (623, 'bconaghyha'),
    (624, 'dfyshhb'),
    (625, 'dhundeyhc'),
    (626, 'uneasamhd'),
    (627, 'emullinshe'),
    (628, 'srubinsohnhf'),
    (629, 'wpricketthg'),
    (630, 'pjeacockhh'),
    (631, 'rlebarreehi'),
    (632, 'tflacknellhj'),
    (633, 'lwelhamhk'),
    (634, 'cgoundrillhl'),
    (635, 'lmaliffehm'),
    (636, 'lconnophn'),
    (637, 'smeakho'),
    (638, 'asuthrenhp'),
    (639, 'mcrouxhq'),
    (640, 'sfearnyhoughhr'),
    (641, 'aboddiehs'),
    (642, 'rkaubleht'),
    (643, 'jkeatshu'),
    (644, 'doskehanhv'),
    (645, 'earenshw'),
    (646, 'ctullethhx'),
    (647, 'sganforthhy'),
    (648, 'stregienhz'),
    (649, 'bbratcheri0'),
    (650, 'rkingsnorthi1'),
    (651, 'mweinmanni2'),
    (652, 'nfurmani3'),
    (653, 'dhurnelli4'),
    (654, 'ndurbanni5'),
    (655, 'gtompioni6'),
    (656, 'mdarragoni7'),
    (657, 'bthowlessi8'),
    (658, 'ktenbrugi9'),
    (659, 'imivalia'),
    (660, 'eizachikib'),
    (661, 'sungereric'),
    (662, 'grisebrowid'),
    (663, 'cbeggsie'),
    (664, 'dbrockingif'),
    (665, 'smacsorleyig'),
    (666, 'tquickfallih'),
    (667, 'sstoveii'),
    (668, 'tleyburnij'),
    (669, 'bvaseninik'),
    (670, 'acleallil'),
    (671, 'ahayballim'),
    (672, 'atrimmingin'),
    (673, 'lmussettiio'),
    (674, 'krathboneip'),
    (675, 'bhaughaniq'),
    (676, 'cgrimsdithir'),
    (677, 'vdienesis'),
    (678, 'sshyramit'),
    (679, 'mmaclleeseiu'),
    (680, 'dnottinghamiv'),
    (681, 'ldedenhamiw'),
    (682, 'kvasnetsovix'),
    (683, 'aabysiy'),
    (684, 'lmckinlayiz'),
    (685, 'tdelcastelj0'),
    (686, 'hivankovicj1'),
    (687, 'eburbidgej2'),
    (688, 'cbaisej3'),
    (689, 'amatasj4'),
    (690, 'idecarolisj5'),
    (691, 'srilingsj6'),
    (692, 'mkenafaquej7'),
    (693, 'rsavinsj8'),
    (694, 'esabyj9'),
    (695, 'cdawidowitschja'),
    (696, 'weddsjb'),
    (697, 'jbohlingjc'),
    (698, 'spatzeltjd'),
    (699, 'ebuncomje'),
    (700, 'kfoottitjf'),
    (701, 'owebbyjg'),
    (702, 'graouxjh'),
    (703, 'aclemenceauji'),
    (704, 'rblazijj'),
    (705, 'lfarthinjk'),
    (706, 'rshynnjl'),
    (707, 'qfrippjm'),
    (708, 'bmallabyjn'),
    (709, 'alattosjo'),
    (710, 'elardezjp'),
    (711, 'loakeshottjq'),
    (712, 'lwinchcombjr'),
    (713, 'pchanterjs'),
    (714, 'jkellowayjt'),
    (715, 'jsoutterju'),
    (716, 'rclawejv'),
    (717, 'qdymockejw'),
    (718, 'clapidusjx'),
    (719, 'kbestwerthickjy'),
    (720, 'heldredjz'),
    (721, 'nhelbeckk0'),
    (722, 'rwarcopk1'),
    (723, 'nbirdallk2'),
    (724, 'bgoodluckk3'),
    (725, 'otedridgek4'),
    (726, 'mgarwoodk5'),
    (727, 'gscahillk6'),
    (728, 'nmodenk7'),
    (729, 'gfearnsk8'),
    (730, 'fvolkk9'),
    (731, 'mkeatchka'),
    (732, 'awillertonkb'),
    (733, 'bglozmankc'),
    (734, 'gagglionekd'),
    (735, 'cmacknockiterke'),
    (736, 'emclartykf'),
    (737, 'tpalakg'),
    (738, 'mmoxtedkh'),
    (739, 'nfreathyki'),
    (740, 'bfiveykj'),
    (741, 'lfrierkk'),
    (742, 'blakendenkl'),
    (743, 'cbullmankm'),
    (744, 'ctokellkn'),
    (745, 'dkeuningko'),
    (746, 'lmorisonkp'),
    (747, 'aroekq'),
    (748, 'wasliekr'),
    (749, 'dargueks'),
    (750, 'dmannockkt'),
    (751, 'evowdonku'),
    (752, 'ldobbskv'),
    (753, 'bpughsleykw'),
    (754, 'gyakovlevkx'),
    (755, 'rgrimolbieky'),
    (756, 'cdallaskz'),
    (757, 'jlivesayl0'),
    (758, 'cpiercel1'),
    (759, 'kbuckel2'),
    (760, 'lrowterl3'),
    (761, 'mbochl4'),
    (762, 'gsimonetonl5'),
    (763, 'anoahl6'),
    (764, 'wdignuml7'),
    (765, 'lpautotl8'),
    (766, 'aslocumbl9'),
    (767, 'hiwaszkiewiczla'),
    (768, 'aautinlb'),
    (769, 'harkelllc'),
    (770, 'hmccoskerld'),
    (771, 'ngumbrellle'),
    (772, 'besparzalf'),
    (773, 'dvaleklg'),
    (774, 'cneseylh'),
    (775, 'jridgewellli'),
    (776, 'rcorserlj'),
    (777, 'nughellilk'),
    (778, 'jroanll'),
    (779, 'ehowatlm'),
    (780, 'gryallln'),
    (781, 'abortolinilo'),
    (782, 'lfardylp'),
    (783, 'cbrassingtonlq'),
    (784, 'osuccamorelr'),
    (785, 'ucrusels'),
    (786, 'slinwoodlt'),
    (787, 'ftavernerlu'),
    (788, 'mmittonlv'),
    (789, 'bdykeslw'),
    (790, 'ryesinovlx'),
    (791, 'ylarmetly'),
    (792, 'tcoltartlz'),
    (793, 'dfallam0'),
    (794, 'champshawm1'),
    (795, 'hvickorsm2'),
    (796, 'ajosselsonm3'),
    (797, 'acapelowm4'),
    (798, 'kelegoodm5'),
    (799, 'lroddm6'),
    (800, 'wkennermannm7'),
    (801, 'fsmedleym8'),
    (802, 'hatherm9'),
    (803, 'dheggesma'),
    (804, 'nstokeymb'),
    (805, 'mgerglermc'),
    (806, 'pleveemd'),
    (807, 'msearightme'),
    (808, 'xvocemf'),
    (809, 'tnisbetmg'),
    (810, 'clissandrinimh'),
    (811, 'hblythmi'),
    (812, 'dfrowmj'),
    (813, 'kofermk'),
    (814, 'zswanneml'),
    (815, 'stumultymm'),
    (816, 'gjuaresmn'),
    (817, 'cmatterseymo'),
    (818, 'tgethinmp'),
    (819, 'jlinfootmq'),
    (820, 'jbrinsdenmr'),
    (821, 'harkwrightms'),
    (822, 'cmulvymt'),
    (823, 'rdingsdalemu'),
    (824, 'ivarnemv'),
    (825, 'bbathersbymw'),
    (826, 'ogertzmx'),
    (827, 'tdominymy'),
    (828, 'gkemptonmz'),
    (829, 'otewkesburyn0'),
    (830, 'tpersentn1'),
    (831, 'ckermottn2'),
    (832, 'awookeyn3'),
    (833, 'klyaln4'),
    (834, 'cjorgern5'),
    (835, 'kparramoren6'),
    (836, 'gmanhoodn7'),
    (837, 'ptremblen8'),
    (838, 'biannin9'),
    (839, 'jdurwardna'),
    (840, 'rquestnb'),
    (841, 'kgildersnc'),
    (842, 'dbourdicend'),
    (843, 'dbidderne'),
    (844, 'bteaznf'),
    (845, 'upleavinng'),
    (846, 'hbrennonnh'),
    (847, 'dterryni'),
    (848, 'ncanningnj'),
    (849, 'scromleynk'),
    (850, 'wlazenburynl'),
    (851, 'jdavenellnm'),
    (852, 'scollettnn'),
    (853, 'ktraskeno'),
    (854, 'tlandsbergnp'),
    (855, 'gmanvellenq'),
    (856, 'lmessenbirdnr'),
    (857, 'khabbershonns'),
    (858, 'gsapautonnt'),
    (859, 'icrudgenu'),
    (860, 'dvinsonnv'),
    (861, 'pjeremaesnw'),
    (862, 'stopingnx'),
    (863, 'kbarnesny'),
    (864, 'ccorderonz'),
    (865, 'rmayberryo0'),
    (866, 'jbrunkero1'),
    (867, 'bhowello2'),
    (868, 'rmactrustrieo3'),
    (869, 'sstrahano4'),
    (870, 'lskilleno5'),
    (871, 'rhunnywello6'),
    (872, 'nsmyo7'),
    (873, 'rgaizeo8'),
    (874, 'kkellso9'),
    (875, 'pliepinaoa'),
    (876, 'pproutob'),
    (877, 'bitzkinoc'),
    (878, 'khambrickod'),
    (879, 'migglesdenoe'),
    (880, 'voharneyof'),
    (881, 'hcawderyog'),
    (882, 'jkleinsternoh'),
    (883, 'zmacguireoi'),
    (884, 'nadamioj'),
    (885, 'gyakuntzovok'),
    (886, 'mchisolmol'),
    (887, 'ccarlesom'),
    (888, 'gturbaton'),
    (889, 'jstobartoo'),
    (890, 'oskaeop'),
    (891, 'rffordeoq'),
    (892, 'kaspeyor'),
    (893, 'budallos'),
    (894, 'cdounbareot'),
    (895, 'lpotterilou'),
    (896, 'jlemmertzov'),
    (897, 'hgrimsterow'),
    (898, 'mcosteox'),
    (899, 'rbrokenshawoy'),
    (900, 'estuckeoz'),
    (901, 'miscowitzp0'),
    (902, 'bcudp1'),
    (903, 'jdurrancep2'),
    (904, 'bmuddimerp3'),
    (905, 'akubalp4'),
    (906, 'xphilippsohnp5'),
    (907, 'plavignep6'),
    (908, 'kmainstonp7'),
    (909, 'lcolbournep8'),
    (910, 'hbellewp9'),
    (911, 'bwigglesworthpa'),
    (912, 'tmccreaghpb'),
    (913, 'ckaesmakerspc'),
    (914, 'zlukockpd'),
    (915, 'vlambdeanpe'),
    (916, 'ljeffcoatepf'),
    (917, 'drojapg'),
    (918, 'kbristonph'),
    (919, 'olodinpi'),
    (920, 'mgoscombpj'),
    (921, 'lgiamellipk'),
    (922, 'ajellybrandpl'),
    (923, 'cbenzpm'),
    (924, 'sirnyspn'),
    (925, 'fkleinhauspo'),
    (926, 'sfollowspp'),
    (927, 'trockinghampq'),
    (928, 'iottawellpr'),
    (929, 'cbyrthps'),
    (930, 'raxcellpt'),
    (931, 'blaINTOnpu'),
    (932, 'fhatrickpv'),
    (933, 'smachanpw'),
    (934, 'fofogartypx'),
    (935, 'cbrokenbrowpy'),
    (936, 'ewoodingpz'),
    (937, 'pilsonq0'),
    (938, 'sbartenq1'),
    (939, 'pmylesq2'),
    (940, 'dryceq3'),
    (941, 'ngilhoolieq4'),
    (942, 'arowlingsq5'),
    (943, 'adenkinq6'),
    (944, 'gleattq7'),
    (945, 'emackneisq8'),
    (946, 'phuyhtonq9'),
    (947, 'ybutcherqa'),
    (948, 'kgudginqb'),
    (949, 'lgureryqc'),
    (950, 'xsharvilleqd'),
    (951, 'eschwaigerqe'),
    (952, 'zclokeqf'),
    (953, 'ifairlemqg'),
    (954, 'belyqh'),
    (955, 'vlefeaverqi'),
    (956, 'rroofqj'),
    (957, 'rseerqk'),
    (958, 'ldrowsfieldql'),
    (959, 'baparkqm'),
    (960, 'emuckeenqn'),
    (961, 'hlansberryqo'),
    (962, 'rpiatkowqp'),
    (963, 'cprandiqq'),
    (964, 'egosdenqr'),
    (965, 'ftethcoteqs'),
    (966, 'ksperingqt'),
    (967, 'dmatterisqu'),
    (968, 'bfolkesqv'),
    (969, 'vzavattieroqw'),
    (970, 'lgumbleyqx'),
    (971, 'mmelsomeqy'),
    (972, 'kbaksterqz'),
    (973, 'idomanr0'),
    (974, 'fguilaemr1'),
    (975, 'bcullinr2'),
    (976, 'etreadgoldr3'),
    (977, 'ncalwayr4'),
    (978, 'wmaccauleyr5'),
    (979, 'tfeehamr6'),
    (980, 'cfosseyr7'),
    (981, 'cperschker8'),
    (982, 'lwaldramr9'),
    (983, 'mbridlera'),
    (984, 'egreenhouserb'),
    (985, 'npotterrc'),
    (986, 'dakriggrd'),
    (987, 'ssandersre'),
    (988, 'glantiffrf'),
    (989, 'cbolingbrokerg'),
    (990, 'tklamprh'),
    (991, 'tshaftori'),
    (992, 'cjaycocksrj'),
    (993, 'llampardrk'),
    (994, 'lbarnetrl'),
    (995, 'chordellrm'),
    (996, 'wcookern'),
    (997, 'hburkro'),
    (998, 'cpoyzerrp'),
    (999, 'gcharetterq'),
    (1000, 'mkennonrr');
SET IDENTITY_INSERT [dbo].[Users] OFF

INSERT INTO [dbo].[Friends] ([User1Id], [User2Id]) VALUES
    (748, 750),
    (242, 768),
    (380, 703),
    (718, 639),
    (792, 640),
    (412, 960),
    (864, 599),
    (487, 142),
    (831, 610),
    (26, 648),
    (108, 647),
    (768, 416),
    (975, 956),
    (50, 668),
    (70, 33),
    (406, 651),
    (318, 962),
    (870, 639),
    (836, 236),
    (279, 564),
    (721, 261),
    (362, 945),
    (209, 167),
    (838, 904),
    (543, 692),
    (298, 151),
    (576, 517),
    (293, 734),
    (988, 414),
    (171, 444),
    (453, 513),
    (135, 89),
    (867, 373),
    (361, 403),
    (379, 94),
    (998, 304),
    (959, 171),
    (643, 479),
    (56, 375),
    (528, 247),
    (165, 493),
    (599, 258),
    (526, 444),
    (729, 842),
    (578, 508),
    (356, 773),
    (352, 885),
    (214, 325),
    (755, 515),
    (998, 605),
    (751, 472),
    (261, 993),
    (910, 842),
    (278, 6),
    (12, 566),
    (393, 728),
    (267, 622),
    (89, 359),
    (302, 513),
    (206, 175),
    (344, 431),
    (701, 403),
    (636, 263),
    (4, 803),
    (247, 214),
    (839, 93),
    (591, 390),
    (920, 848),
    (370, 9),
    (623, 341),
    (122, 52),
    (928, 577),
    (313, 935),
    (330, 763),
    (590, 86),
    (233, 318),
    (7, 499),
    (932, 945),
    (813, 231),
    (67, 922),
    (128, 313),
    (87, 104),
    (662, 951),
    (262, 620),
    (310, 720),
    (546, 952),
    (748, 826),
    (181, 19),
    (97, 733),
    (526, 10),
    (305, 212),
    (91, 506),
    (359, 908),
    (798, 804),
    (148, 531),
    (275, 446),
    (102, 394),
    (631, 259),
    (523, 74),
    (460, 709);
