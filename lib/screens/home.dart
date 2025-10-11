import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import '../widgets/app_colors.dart';
import '../widgets/app_background.dart';

class Home extends StatelessWidget {
  const Home({super.key});

  void _showNotifications(BuildContext context) {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return Dialog(
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
          insetPadding: const EdgeInsets.symmetric(horizontal: 30, vertical: 24),
          child: Container(
            padding: const EdgeInsets.all(20),
            decoration: BoxDecoration(
              gradient: LinearGradient(
                colors: [
                  AppColors.primaryGradient.colors.first.withOpacity(0.15),
                  AppColors.primaryGradient.colors.last.withOpacity(0.15),
                ],
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
              borderRadius: BorderRadius.circular(20),
            ),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                ShaderMask(
                  shaderCallback: (bounds) =>
                      AppColors.primaryGradient.createShader(bounds),
                  child: const Text(
                    "Notifications",
                    style: TextStyle(
                      fontSize: 22,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                ),
                const SizedBox(height: 20),
                _buildNotification("Stay Hydrated!"),
                ElevatedButton(
                  onPressed: () => Navigator.pop(context),
                  style: ElevatedButton.styleFrom(
                    backgroundColor: AppColors.primaryGradient.colors.first,
                    shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(14)),
                    padding: const EdgeInsets.symmetric(
                        horizontal: 24, vertical: 10),
                  ),
                  child: const Text(
                    "Close",
                    style: TextStyle(fontSize: 16, color: Colors.white),
                  ),
                ),
              ],
            ),
          ),
        );
      },
    );
  }

  static Widget _buildNotification(String text) {
    return Container(
      margin: const EdgeInsets.only(bottom: 10),
      padding: const EdgeInsets.all(14),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(12),
        border: Border.all(
          color: AppColors.primaryGradient.colors.first.withOpacity(0.3),
          width: 1.5,
        ),
      ),
      child: Row(
        children: [
          ShaderMask(
            shaderCallback: (bounds) =>
                AppColors.primaryGradient.createShader(bounds),
            child: const Icon(Icons.notifications_active_rounded,
                color: Colors.white, size: 22),
          ),
          const SizedBox(width: 10),
          Expanded(
            child: Text(
              text,
              style: const TextStyle(
                color: Colors.black87,
                fontSize: 15,
                fontWeight: FontWeight.w500,
              ),
            ),
          ),
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: AppBackground(
        child: SafeArea(
          child: Padding(
            padding: const EdgeInsets.symmetric(horizontal: 24.0),
            child: ListView(
              children: [
                const SizedBox(height: 24),
                const _Header(),
                const SizedBox(height: 30),
                const _AlertCard(),
                const SizedBox(height: 20),
                _SearchAndNotification(onNotifyTap: () {
                  _showNotifications(context);
                }),
                const SizedBox(height: 24),
                const _QuickActionsRow(),
                const SizedBox(height: 22),
              ],
            ),
          ),
        ),
      ),
    );
  }
}

class _Header extends StatelessWidget {
  const _Header();

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Row(
          children: [
            ShaderMask(
              shaderCallback: (Rect bounds) =>
                  AppColors.primaryGradient.createShader(bounds),
              child: SvgPicture.asset(
                'assets/ACLC.svg',
                height: 46,
                width: 46,
                color: Colors.white,
              ),
            ),
            const SizedBox(width: 8),
            ShaderMask(
              shaderCallback: (bounds) =>
                  AppColors.primaryGradient.createShader(bounds),
              child: const Text(
                'Hello, Nurse!',
                style: TextStyle(
                  color: Colors.white,
                  fontSize: 26,
                  fontWeight: FontWeight.w700,
                  letterSpacing: 1,
                ),
              ),
            ),
          ],
        ),
        PopupMenuButton<String>(
          padding: EdgeInsets.zero,
          offset: const Offset(0, 40),
          color: Colors.white,
          shape:
              RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
          child: ShaderMask(
            shaderCallback: (bounds) =>
                AppColors.primaryGradient.createShader(bounds),
            child: const CircleAvatar(
              radius: 22,
              backgroundColor: Colors.transparent,
              child: Icon(
                Icons.person,
                color: Colors.white,
                size: 26,
              ),
            ),
          ),
          itemBuilder: (_) => [
            _menuItem('account', Icons.account_circle, 'Account', Colors.black87),
            _menuItem('logout', Icons.logout, 'Logout', Colors.red),
          ],
          onSelected: (value) {
            if (value == 'account') {
              Navigator.pushNamed(context, '/userinfo');
            } else if (value == 'logout') {
              Navigator.pushReplacementNamed(context, '/signin');
            }
          },
        ),
      ],
    );
  }

  static PopupMenuItem<String> _menuItem(
      String value, IconData icon, String text, Color color) {
    return PopupMenuItem(
      value: value,
      child: Row(
        children: [
          Icon(icon, size: 20, color: color),
          const SizedBox(width: 8),
          Text(
            text,
            style: TextStyle(color: color, fontWeight: FontWeight.w500),
          ),
        ],
      ),
    );
  }
}

class _AlertCard extends StatelessWidget {
  const _AlertCard();

  @override
  Widget build(BuildContext context) {
    return _GradientContainer(
      colors: const [Color(0xFFFF6961), Color(0xFFFF8A80)],
      child: Row(
        children: const [
          Icon(
            Icons.warning_rounded,
            size: 32,
            color: Colors.white,
          ),
          SizedBox(width: 16),
          Expanded(
            child: Text(
              'Urgent Alert: Seizure episode reported. Review medication immediately.',
              style: TextStyle(
                color: Colors.white,
                fontSize: 16,
                fontWeight: FontWeight.w600,
              ),
            ),
          ),
        ],
      ),
    );
  }
}

class _SearchAndNotification extends StatelessWidget {
  final VoidCallback onNotifyTap;
  const _SearchAndNotification({required this.onNotifyTap});

  @override
  Widget build(BuildContext context) {
    return Row(
      children: [
        Expanded(
          child: _GradientContainer(
            colors: AppColors.primaryGradient.colors,
            padding: const EdgeInsets.symmetric(horizontal: 16),
            child: TextField(
              decoration: InputDecoration(
                hintText: 'Search patients',
                hintStyle: TextStyle(color: Colors.white.withOpacity(0.7)),
                prefixIcon: const Icon(
                  Icons.search,
                  color: Colors.white,
                  size: 28,
                ),
                prefixIconConstraints: const BoxConstraints(
                  minHeight: 40,
                  minWidth: 40,
                  maxHeight: 40,
                ),
                contentPadding: const EdgeInsets.symmetric(vertical: 12),
                border: InputBorder.none,
              ),
              style: const TextStyle(color: Colors.white, fontSize: 16),
            ),
          ),
        ),
        const SizedBox(width: 15),
        InkWell(
          onTap: onNotifyTap,
          borderRadius: BorderRadius.circular(16),
          child: _GradientContainer(
            colors: AppColors.primaryGradient.colors,
            padding: const EdgeInsets.all(12),
            child: const Icon(
              Icons.notifications,
              size: 26,
              color: Colors.white,
            ),
          ),
        ),
      ],
    );
  }
}

class _QuickActionsRow extends StatelessWidget {
  const _QuickActionsRow();

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        ShaderMask(
          shaderCallback: (bounds) =>
              AppColors.primaryGradient.createShader(bounds),
          child: const Text(
            'Quick Actions',
            style: TextStyle(
              fontSize: 20,
              fontWeight: FontWeight.bold,
              color: Colors.white,
            ),
          ),
        ),
        const SizedBox(height: 16),
        GridView.count(
          crossAxisCount: 2,
          shrinkWrap: true,
          physics: const NeverScrollableScrollPhysics(),
          crossAxisSpacing: 16,
          mainAxisSpacing: 16,
          childAspectRatio: 1.0,
          children: const [
            _SquareActionCard('Appointments', Icons.calendar_today_rounded),
            _SquareActionCard('Patient Records', Icons.folder_shared_rounded),
            _SquareActionCard('Medical Inventory', Icons.medication_rounded),
            _SquareActionCard('Add Form', Icons.add_box_rounded),
          ],
        ),
      ],
    );
  }
}

class _SquareActionCard extends StatelessWidget {
  final String title;
  final IconData icon;

  const _SquareActionCard(this.title, this.icon);

  @override
  Widget build(BuildContext context) {
    return InkWell(
      onTap: () {
        if (title == 'Appointments') {
          Navigator.pushNamed(context, '/appointments');
        } else if (title == 'Patient Records') {
          Navigator.pushNamed(context, '/patientsrecords');
        } else if (title == 'Medical Inventory') {
          Navigator.pushNamed(context, '/medicalinventory');
        } else if (title == 'Add Form') {
          Navigator.pushNamed(context, '/addform');
        }
      },
      borderRadius: BorderRadius.circular(16),
      child: _GradientContainer(
        colors: AppColors.primaryGradient.colors,
        padding: const EdgeInsets.all(20),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(icon, size: 40, color: Colors.white),
            const SizedBox(height: 12),
            Text(
              title,
              textAlign: TextAlign.center,
              style: const TextStyle(
                color: Colors.white,
                fontSize: 15,
                fontWeight: FontWeight.w500,
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class _GradientContainer extends StatelessWidget {
  final Widget child;
  final List<Color> colors;
  final EdgeInsets padding;

  const _GradientContainer({
    required this.child,
    required this.colors,
    this.padding = const EdgeInsets.all(16),
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: BoxDecoration(
        gradient: LinearGradient(
          colors: colors,
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: Colors.white.withOpacity(0.3), width: 2),
      ),
      child: Padding(
        padding: padding,
        child: child,
      ),
    );
  }
}
