import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:flutter_temp/widgets/app_colors.dart';
import 'package:flutter_temp/widgets/app_background.dart';
import 'userinfo.dart';
import '../landing/signin.dart';
import 'medical_record.dart';

class MedicalRecordModel {
  final String date;
  final String activity;
  final String scheduledTime;
  final String doctor;

  const MedicalRecordModel({
    required this.date,
    required this.activity,
    required this.scheduledTime,
    required this.doctor,
  });
}

const List<MedicalRecordModel> allRecords = [
  MedicalRecordModel(date: 'January 4th, 2018', activity: 'Dental hygiene', scheduledTime: '9:00am', doctor: 'Nurse Chavez'),
  MedicalRecordModel(date: 'December 17th, 2017', activity: 'Sore throat checkup', scheduledTime: '10:30am', doctor: 'Nurse Rai'),
  MedicalRecordModel(date: 'August 21th, 2017', activity: 'Circulatory problems', scheduledTime: '4:45pm', doctor: 'Nurse AYumi'),
  MedicalRecordModel(date: 'July 10th, 2017', activity: 'Blood pressure check', scheduledTime: '2:00pm', doctor: 'Nurse Jan'),
  MedicalRecordModel(date: 'July 10th, 2017', activity: 'Blood pressure check', scheduledTime: '2:00pm', doctor: 'Nurse Inot'),
  MedicalRecordModel(date: 'July 10th, 2017', activity: 'Blood pressure check', scheduledTime: '2:00pm', doctor: 'Nurse Gab'),
];

class Home extends StatefulWidget {
  const Home({super.key});

  @override
  State<Home> createState() => _HomeState();
}

class _HomeState extends State<Home> {
  Offset fabPosition = const Offset(320, 600);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: AppBackground(
        child: Stack(
          children: [
            SafeArea(
              child: Padding(
                padding: const EdgeInsets.symmetric(horizontal: 24, vertical: 16),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    const SizedBox(height: 12),
                    const _Header(),
                    const SizedBox(height: 24),
                    ShaderMask(
                    shaderCallback: (bounds) => AppColors.primaryGradient.createShader(bounds),
                    blendMode: BlendMode.srcIn,
                    child: const Text(
                      "Medical Record",
                      style: TextStyle(
                        fontSize: 20,
                        fontWeight: FontWeight.bold,
                        color: Color.fromARGB(255, 255, 255, 255), 
                      ),
                    ),
                  ),
                  const SizedBox(height: 16),
                  Expanded(child: _PatientRecordSection(allRecords: allRecords)),
                  ],
                ),
              ),
            ),
            Positioned(
              left: fabPosition.dx,
              top: fabPosition.dy,
              child: Draggable(
                feedback: const Material(color: Colors.transparent, child: FloatingChatIcon()),
                childWhenDragging: const SizedBox.shrink(),
                onDragEnd: (details) {
                  setState(() {
                    final size = MediaQuery.of(context).size;
                    double x = details.offset.dx.clamp(0.0, size.width - 72);
                    double y = details.offset.dy.clamp(0.0, size.height - 72);
                    fabPosition = Offset(x, y);
                  });
                },
                child: const FloatingChatIcon(),
              ),
            ),
          ],
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
            SizedBox(
              height: 80,
              width: 80,
              child: ShaderMask(
                shaderCallback: (bounds) => AppColors.primaryGradient.createShader(bounds),
                blendMode: BlendMode.srcIn,
                child: SvgPicture.asset(
                  'assets/ACLC.svg',
                  fit: BoxFit.contain,
                  placeholderBuilder: (context) => const Icon(Icons.school, color: Colors.white, size: 48),
                ),
              ),
            ),
            const SizedBox(width: 12),
            ShaderMask(
              shaderCallback: (bounds) => AppColors.primaryGradient.createShader(bounds),
              child: const Text(
                "Hello, Ayums!",
                style: TextStyle(fontSize: 28, fontWeight: FontWeight.w900, color: Colors.white),
              ),
            ),
          ],
        ),
        PopupMenuButton<String>(
          padding: EdgeInsets.zero,
          offset: const Offset(0, 40),
          color: Colors.white,
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
          child: Container(
            decoration: const BoxDecoration(shape: BoxShape.circle, gradient: AppColors.primaryGradient),
            child: const CircleAvatar(radius: 20, backgroundColor: Colors.transparent, child: Icon(Icons.person, color: Colors.white)),
          ),
          itemBuilder: (_) => [
            _menuItem('account', Icons.person, 'Account', AppColors.black),
            _menuItem('logout', Icons.logout, 'Logout', AppColors.accentRed),
          ],
          onSelected: (value) {
            if (value == 'account') {
              Navigator.push(context, MaterialPageRoute(builder: (_) => UserInfo()));
            } else if (value == 'logout') {
              Navigator.pushReplacement(context, MaterialPageRoute(builder: (_) => SignIn()));
            }
          },
        ),
      ],
    );
  }

  PopupMenuItem<String> _menuItem(String value, IconData icon, String text, Color color) {
    return PopupMenuItem(
      value: value,
      child: Row(
        children: [
          Icon(icon, color: color, size: 20),
          const SizedBox(width: 8),
          Text(text, style: TextStyle(color: color, fontWeight: FontWeight.w600)),
        ],
      ),
    );
  }
}

class FloatingChatIcon extends StatelessWidget {
  const FloatingChatIcon({super.key});

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {},
      child: Container(
        padding: const EdgeInsets.all(14),
        decoration: BoxDecoration(
          shape: BoxShape.circle,
          gradient: AppColors.primaryGradient,
          boxShadow: const [BoxShadow(color: Color.fromARGB(126, 32, 66, 124), blurRadius: 12, spreadRadius: 2)],
        ),
        child: const Icon(Icons.chat_bubble_outline, size: 28, color: Colors.white),
      ),
    );
  }
}

class _PatientRecordSection extends StatelessWidget {
  final List<MedicalRecordModel> allRecords;
  const _PatientRecordSection({required this.allRecords});

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      padding: EdgeInsets.zero,
      itemCount: allRecords.length,
      itemBuilder: (context, index) {
        return Padding(
          padding: const EdgeInsets.only(bottom: 16),
          child: _PatientRecordTile(record: allRecords[index]),
        );
      },
    );
  }
}

class _PatientRecordTile extends StatelessWidget {
  final MedicalRecordModel record;
  const _PatientRecordTile({required this.record});

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {
        Navigator.push(context, MaterialPageRoute(builder: (_) => const MedicalRecord()));
      },
      child: Container(
        decoration: BoxDecoration(
          gradient: AppColors.primaryGradient,
          borderRadius: BorderRadius.circular(12),
          boxShadow: [BoxShadow(color: Colors.black.withOpacity(0.1), blurRadius: 8, offset: const Offset(0, 4))],
        ),
        padding: const EdgeInsets.all(16),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    record.date,
                    style: const TextStyle(color: Colors.white60, fontSize: 14, fontWeight: FontWeight.w400),
                  ),
                  const SizedBox(height: 6),
                  Text(
                    record.activity,
                    style: const TextStyle(color: Colors.white, fontSize: 16, fontWeight: FontWeight.w600),
                  ),
                  const SizedBox(height: 4),
                  Text(
                    'Scheduled: ${record.scheduledTime}',
                    style: const TextStyle(color: Colors.white70, fontWeight: FontWeight.w500),
                  ),
                  const SizedBox(height: 2),
                  Text(
                    'Doctor: ${record.doctor}',
                    style: const TextStyle(color: Colors.white70, fontWeight: FontWeight.w500),
                  ),
                ],
              ),
            ),
            const Icon(Icons.arrow_forward_ios, size: 16, color: Colors.white),
          ],
        ),
      ),
    );
  }
}
