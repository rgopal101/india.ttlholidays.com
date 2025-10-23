-- phpMyAdmin SQL Dump
-- version 5.2.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Oct 23, 2025 at 05:38 AM
-- Server version: 8.0.44
-- PHP Version: 8.4.13

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `apinta_ntaflee`
--

-- --------------------------------------------------------

--
-- Table structure for table `menu`
--

CREATE TABLE `menu` (
  `id` int NOT NULL,
  `name` varchar(60) CHARACTER SET utf8mb3 COLLATE utf8mb3_unicode_ci NOT NULL,
  `link` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_unicode_ci NOT NULL,
  `idmenu` int NOT NULL,
  `dated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ord` int NOT NULL,
  `icon` varchar(30) CHARACTER SET utf8mb3 COLLATE utf8mb3_unicode_ci NOT NULL,
  `del` int NOT NULL,
  `idby` int NOT NULL,
  `sw` tinyint NOT NULL DEFAULT '0'
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_unicode_ci;

--
-- Dumping data for table `menu`
--

INSERT INTO `menu` (`id`, `name`, `link`, `idmenu`, `dated`, `ord`, `icon`, `del`, `idby`, `sw`) VALUES
(1, 'Dashboard', '#', 0, '2019-12-12 13:19:16', 1, 'dashboard', 0, 2, 0),
(2, 'View Discounts', '#', 0, '2019-12-12 15:24:11', 2, 'discount', 0, 2, 0),
(3, 'Retail Prices', '#', 0, '2019-12-12 15:24:44', 3, 'price', 0, 2, 0),
(4, 'Location', '#', 0, '2019-12-12 15:24:52', 14, 'location', 0, 2, 0),
(5, 'Items', '/OwnerUpdate_rack_price', 0, '2019-12-12 15:25:01', 5, 'item', 0, 2, 0),
(6, 'Company', '#', 0, '2019-12-12 15:25:07', 6, 'company', 0, 2, 0),
(7, 'Fuel Cards', '#', 0, '2019-12-12 15:25:13', 7, 'fuel', 0, 2, 0),
(8, 'Transactions', '#', 0, '2019-12-12 15:25:20', 8, 'transaction', 0, 2, 0),
(9, 'Invoices', '#', 0, '2019-12-12 15:25:26', 10, 'invoices', 0, 2, 0),
(10, 'Reporting', '#', 0, '2019-12-12 15:25:32', 11, 'report', 0, 2, 0),
(11, 'Supplier', '#', 0, '2019-12-12 15:25:38', 12, 'supply', 0, 2, 0),
(12, 'Money Code', '#', 0, '2019-12-12 15:25:45', 13, 'moneyCode', 0, 2, 0),
(13, 'Setting', '#', 0, '2019-12-12 15:25:50', 15, 'setting', 0, 2, 0),
(14, 'Dashboard', '/default', 1, '2019-12-12 15:28:28', 1, '', 0, 2, 0),
(15, 'Report Dashboard', '/report_Dashboard', 1, '2019-12-12 15:29:46', 2, '', 0, 2, 0),
(16, 'View Discounts', '/view_Discount', 2, '2019-12-12 15:30:18', 2, '', 0, 2, 0),
(17, 'Upload Retail Prices', '/upload_retail_prices', 3, '2019-12-12 15:30:37', 1, '', 0, 2, 0),
(18, 'View Countries', 'view_countries.php', 4, '2019-12-12 15:30:55', 1, '', 0, 2, 0),
(19, 'View States', 'view_states.php', 4, '2019-12-12 15:31:40', 2, '', 0, 2, 0),
(20, 'View Cities', 'view_cities.php', 4, '2019-12-12 15:31:54', 3, '', 0, 2, 0),
(21, 'View Items', '/view_items', 5, '2019-12-12 15:32:27', 0, '', 0, 2, 0),
(22, 'Add Items', '/add_items', 5, '2019-12-12 15:32:42', 0, '', 0, 2, 0),
(23, 'View Company', '/view_company', 6, '2019-12-12 15:33:42', 2, '', 0, 2, 0),
(24, 'Add Company', '/add_company', 6, '2019-12-12 15:33:53', 1, '', 0, 2, 0),
(25, 'View Fuel Cards', '/view_fuelCards', 7, '2019-12-12 15:34:06', 0, '', 0, 2, 0),
(26, 'Add Fuel Card', 'add_card.php', 7, '2019-12-12 15:34:18', 0, '', 0, 2, 0),
(27, 'Upload Fuel Card', 'upload_card.php', 7, '2019-12-12 15:34:29', 0, '', 1, 2, 0),
(28, 'View Transactions', '/view_transaction', 8, '2019-12-12 15:34:41', 1, '', 0, 2, 0),
(29, 'Unknown Transactions', '/unknown_transaction', 8, '2019-12-12 15:34:52', 4, '', 0, 2, 0),
(112, 'Download Bulk Excel', '/download_bulk_exce', 54, '2021-06-14 16:21:12', 16, '', 0, 1, 0),
(31, 'Upload Transactions', '/upload_transaction', 8, '2019-12-12 15:35:24', 2, '', 0, 2, 0),
(32, 'View Invoices', '/view_invoice', 9, '2019-12-12 15:36:26', 1, '', 0, 2, 0),
(33, 'Create New Invoice', 'create_invoice.php?uf=0', 9, '2019-12-12 15:36:38', 2, '', 0, 2, 0),
(34, 'Create Bulk Invoice', 'create_bulk_invoice.php', 9, '2019-12-12 15:36:48', 3, '', 1, 2, 0),
(35, 'Send Bulk Mail', 'send_bulk_mail.php', 9, '2019-12-12 15:37:08', 4, '', 0, 2, 0),
(36, 'View Reports', 'viewreports.php?uf=0', 10, '2019-12-12 15:37:24', 2, '', 0, 2, 0),
(37, 'Create Reports', 'createreports.php?uf=0', 10, '2019-12-12 15:37:34', 1, '', 0, 2, 0),
(38, 'Supplier List', '/suppler_list', 11, '2019-12-12 15:37:52', 0, '', 0, 2, 0),
(39, 'Add Supplier', '/add_supplier', 11, '2019-12-12 15:38:13', 0, '', 0, 2, 0),
(40, 'Money Code List', '/money_code_List', 12, '2019-12-12 15:38:31', 1, '', 0, 2, 0),
(41, 'Add Money Code', 'addMoney_code_List', 12, '2019-12-12 15:38:44', 2, '', 0, 2, 0),
(42, 'Upload Money Code', '/upload_money_code', 12, '2019-12-12 15:38:55', 3, '', 0, 2, 0),
(43, 'Manage Menu', 'manage_menu.php', 13, '2019-12-12 15:39:16', 3, '', 0, 2, 0),
(44, 'Manage User', 'manage_user.php', 13, '2019-12-12 15:39:33', 1, '', 0, 2, 0),
(45, 'Petro Retail Prices', '/petro_retail_prices', 3, '2019-12-18 18:45:53', 2, '', 0, 1, 0),
(46, 'Create Discount ', '/create_discount', 2, '2019-12-20 00:59:05', 1, '', 0, 1, 0),
(111, 'User Login Log', 'login_log.php', 13, '2021-05-11 14:39:28', 0, '', 0, 1, 0),
(110, 'ULTRAMAR Group Rack Pricing', '/ultramarGroup_rack_pric', 54, '2021-05-06 13:45:47', 15, '', 0, 1, 0),
(49, 'Help', '#', 0, '2020-01-01 11:21:38', 17, 'help', 0, 1, 0),
(50, 'How to Add Card..?', 'help_add_card.php', 49, '2020-01-01 11:26:05', 2, '', 0, 1, 0),
(51, 'How to use efsllc.com With NTA..?', 'help_use_efsllc.php', 49, '2020-01-01 11:29:30', 1, '', 0, 1, 0),
(53, 'Discount Sheet', '/discount_Sheet', 2, '2020-01-13 16:43:05', 3, '', 0, 1, 0),
(52, 'Check Old Invoices', '/check_invoice', 9, '2020-01-09 16:03:28', 5, '', 0, 1, 0),
(54, 'Pricing', '#', 0, '2020-01-18 15:28:39', 4, 'price', 0, 1, 0),
(55, 'Upload Pricing', '/upload_price', 54, '2020-01-18 15:30:06', 1, '', 0, 1, 0),
(56, 'Pricing List', '/price_list', 54, '2020-01-18 15:30:38', 2, '', 0, 1, 0),
(57, 'Single Pricing PDF', '/single_price_pdf', 54, '2020-01-18 15:31:23', 3, '', 0, 1, 0),
(60, 'How to create report at client panel..?', 'how_create_report.php', 49, '2020-02-06 08:58:05', 3, '', 0, 1, 0),
(59, 'FJ Bulk Pricing', '/fj_bulk_price', 54, '2020-02-01 14:12:10', 4, '', 0, 1, 0),
(61, 'How to create card discount sheet at client panel..?', 'how_card_discount.php', 49, '2020-02-06 08:58:55', 4, '', 0, 1, 0),
(62, 'T check', '#', 0, '2020-02-10 13:22:19', 16, 'tcheck', 0, 1, 0),
(63, 'Upload T Check', 'upload_tcheck.php', 62, '2020-02-10 13:23:01', 1, '', 0, 1, 0),
(64, 'T Check List', 'tcheck_list.php', 62, '2020-02-10 13:23:31', 2, '', 0, 1, 0),
(65, 'Manage Sales Man', 'salesman.php', 13, '2020-03-10 22:06:32', 2, '', 0, 1, 0),
(66, 'Salesman Volume Report', 'salesman_report.php', 10, '2020-03-14 15:07:24', 3, '', 0, 1, 0),
(67, 'Check Transactions', '/check_transaction', 8, '2020-04-14 22:08:00', 6, '', 0, 1, 0),
(68, 'Create Repeat Invoice', 'create_repeat_invoice.php', 9, '2020-04-15 09:09:38', 6, '', 0, 1, 0),
(69, 'Create MoneyCode Invoice', 'create_moneycode_invoice.php', 9, '2020-04-22 11:47:49', 9, '', 0, 1, 0),
(70, 'View MoneyCode Invoice', 'view_moneycode_invoices.php', 9, '2020-04-22 11:48:26', 7, '', 0, 1, 0),
(71, 'MoneyCode Send Bulk Mail', 'moneycode_bulk_mail.php', 9, '2020-04-23 15:33:30', 8, '', 0, 1, 0),
(72, 'Check Invoiced MoneyCode', 'check_moneycode.php', 12, '2020-04-27 13:13:28', 4, '', 0, 1, 0),
(73, 'Rebate|Cheques', '#', 0, '2020-05-11 00:08:29', 9, 'fa fa-usd', 0, 1, 0),
(74, 'Single Rebate Entry', 'single_rebate.php', 73, '2020-05-11 00:17:21', 1, '', 0, 1, 0),
(75, 'Multiple Rebate Entry', 'multiple_rebate.php', 73, '2020-05-11 00:17:48', 2, '', 0, 1, 0),
(76, 'Rebate or Cheques List', 'rebate_list.php', 73, '2020-05-11 00:18:59', 3, '', 0, 1, 0),
(109, 'Graph Dashboard', '/graph_Dashboard', 1, '2021-04-27 18:05:44', 3, '', 0, 1, 0),
(79, 'Retail To Rack Transaction', 'to_retail_transaction.php?uf=1', 8, '2020-05-29 14:46:50', 10, '', 0, 1, 0),
(80, 'View Rack Transaction', 'retail_transaction.php', 8, '2020-05-29 14:47:35', 9, '', 0, 1, 0),
(83, 'Update FG Rack Pricing', '/update_fg_rack_price', 54, '2020-06-10 20:51:24', 9, '', 0, 1, 0),
(81, 'View Rack Invoice', 'retail_invoice.php', 9, '2020-05-29 14:50:56', 10, '', 0, 1, 0),
(82, 'Create Rack Invoice', 'create_retail_invoice.php?uf=0', 9, '2020-05-29 14:51:52', 10, '', 0, 1, 0),
(84, 'Create Bulk Discount', '/create_Bulk_Discount', 2, '2020-07-06 13:28:46', 4, '', 0, 1, 0),
(85, 'Create Esso Invoice', 'create_esso_invoice.php?uf=0', 9, '2020-07-20 14:38:50', 11, '', 0, 1, 0),
(86, 'View EFS Transactions', '/view_efs_transaction', 8, '2020-07-27 10:46:04', 12, '', 0, 1, 0),
(87, 'Update Ta-Petro Rack Pricing', '/update_taPetro_rack_price', 54, '2020-08-03 16:19:37', 10, '', 0, 1, 0),
(88, 'TA-Petro Bulk Pricing', '/ta_petro_bulk_price', 54, '2020-08-06 13:33:39', 5, '', 0, 1, 0),
(89, 'ESSO Bulk Pricing', '/esso_bulk_price', 54, '2020-08-06 13:34:04', 6, '', 0, 1, 0),
(90, 'Update ESSO Rack Pricing', '/Update ESSO Rack Pricing', 54, '2020-08-06 13:41:33', 11, '', 0, 1, 0),
(91, 'Rack To Retail Transaction', 'rack_to_retail_transaction.php?uf=0', 8, '2020-08-11 17:04:42', 11, '', 0, 1, 0),
(92, 'Manage Location', 'manage_location.php?uf=0', 4, '2020-08-18 16:01:29', 4, '', 0, 1, 0),
(93, 'Love Bulk Pricing', '/love_bulk_price', 54, '2020-09-01 17:33:18', 7, '', 0, 1, 0),
(94, 'Update LOVE Rack Pricing', '/update_love_rack_pricing', 54, '2020-09-01 17:33:56', 12, '', 0, 1, 0),
(95, 'Create Report New', 'create_report.php?uf=0', 10, '2020-09-12 20:32:40', 4, '', 1, 1, 0),
(96, 'View Report New', 'view_report.php?uf=0', 10, '2020-09-25 13:26:23', 5, '', 1, 1, 0),
(97, 'Owner Operator Rack Pricing', '/OwnerUpdate_rack_price', 54, '2020-10-26 21:05:05', 13, '', 0, 1, 0),
(102, 'ESSO FTP To Live Transactions', '/Esso_ftp_liveTransaction', 8, '2020-11-09 13:03:36', 5, '', 0, 1, 0),
(101, 'ESSO FTP Transactions List', '/Esso_ftp_transaction', 8, '2020-11-05 12:11:24', 3, '', 0, 1, 0),
(100, 'Create Old Invoice', 'create_old_invoice.php?uf=0', 9, '2020-10-26 21:55:23', 13, '', 0, 1, 0),
(103, 'View T-Check Invoice', 'view_tcheck_invoices.php', 62, '2020-12-11 17:45:33', 4, '', 0, 1, 0),
(104, 'Create T-Check Invoice', 'create_tcheck_invoice.php', 62, '2020-12-11 17:46:18', 3, '', 0, 1, 0),
(105, 'ESSO City Rack Pricing', '/ESSO City Rack Pricing', 54, '2020-12-30 10:03:25', 10, '', 0, 1, 0),
(106, 'Manage Group', 'manage_group.php?uf=0', 4, '2021-03-04 06:38:52', 5, '', 0, 1, 0),
(107, 'ESSO Group Rack Pricing', '/esso_group_rack_price', 54, '2021-03-08 18:26:11', 14, '', 0, 1, 0),
(108, 'ULTRAMAR Bulk Pricing', '/ultramar_bulk_price', 54, '2021-03-19 14:28:18', 8, '', 0, 1, 0),
(113, 'Zero Discount Location', '/zero_Discount_Location?uf=0', 2, '2021-06-23 15:32:47', 5, '', 0, 1, 0),
(114, 'Notification', '#', 0, '2021-07-05 17:20:51', 18, 'fa fa-bell', 0, 1, 0),
(115, 'Manage Notification', 'manage_notification.php', 114, '2021-07-05 17:27:30', 1, '', 0, 1, 0),
(116, 'Company Info', '/company_info', 6, '2021-08-13 16:22:09', 3, '', 0, 1, 0),
(117, 'Create Customised Invoice', 'create_customized_invoice.php', 9, '2021-11-08 12:44:41', 14, '', 0, 1, 0),
(118, 'ESSO Group Rack Pricing (New)', '/esso_group_rack_price', 54, '2021-12-13 17:30:52', 14, '', 0, 1, 0),
(119, 'Create ULTRAMAR  Invoice', 'create_ul_invoice.php?uf=0', 9, '2020-07-20 14:38:50', 11, '', 0, 1, 0),
(120, 'View Invoices New', '/View Invoices New', 9, '2019-12-12 15:36:26', 2, '', 0, 2, 0),
(121, 'Manage Macro', 'manage_macro.php', 9, '2023-01-16 07:46:43', 15, '', 0, 1, 0),
(122, 'Linamar Esso Location', 'linamar_esso_location.php', 4, '2023-02-27 15:11:59', 0, '', 0, 1, 0),
(123, 'Last 24 Hours Update', 'card_update.php', 7, '2023-05-08 08:31:10', 4, '', 0, 1, 0),
(124, 'Esso Ultramar Petro Link', 'esso_ultramar_petro_link.php?uf=0', 4, '2023-05-15 13:46:16', 0, '', 0, 1, 0),
(125, 'Compare Invoice', 'compare_invoice.php', 9, '2023-06-10 09:16:09', 16, '', 0, 1, 0),
(126, 'Update Unit / Driver', 'update_unit.php', 8, '2023-06-26 08:31:49', 13, '', 0, 1, 0),
(127, 'Manage Esso Cent Type', '/manage_esso_cent', 54, '2023-10-06 06:23:59', 0, '', 0, 1, 0),
(128, 'Update Esso Cent Wise', '/update_esso_centwise', 54, '2023-10-06 07:36:32', 0, '', 0, 1, 0),
(129, 'Download Esso Cent', '/download_esso_cent', 54, '2023-10-18 12:04:23', 0, '', 0, 21, 0),
(130, 'Manage Sub-Login', 'sub_login.php', 6, '2023-12-20 06:54:33', 4, '', 0, 1, 0),
(131, 'Update Fuel Card', 'edit_card.php', 7, '2023-12-30 13:55:18', 0, '', 0, 1, 1),
(132, 'Edit Transactions', '/ 	Edit Transactions', 8, '2023-12-30 14:02:46', 0, '', 0, 1, 1),
(133, 'Update Company', '/Update Company', 6, '2023-12-30 14:05:24', 0, '', 0, 1, 1),
(134, 'Update T Check', 'edit_tcheck.php', 62, '2023-12-30 14:07:40', 0, '', 0, 1, 1),
(135, 'Update Money Code', '/Update Money Code', 12, '2023-12-30 14:13:58', 0, '', 0, 1, 1),
(136, 'Update Rebate', 'edit_rebate.php', 73, '2023-12-30 14:17:43', 0, '', 0, 1, 1),
(137, 'Track Visitors', 'track_visitors.php', 13, '2023-12-30 14:21:46', 0, '', 0, 1, 0),
(138, 'Company Log', '/Company Log', 13, '2024-02-10 12:46:30', 0, '', 0, 1, 0),
(139, 'EFS API Fuel Card List', 'efs_view_card.php', 7, '2024-12-30 10:00:18', 0, '', 0, 1, 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `menu`
--
ALTER TABLE `menu`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `menu`
--
ALTER TABLE `menu`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=140;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
