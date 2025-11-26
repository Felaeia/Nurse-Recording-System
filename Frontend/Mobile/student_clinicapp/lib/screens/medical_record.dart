import 'package:flutter/material.dart';
import 'package:flutter_temp/widgets/app_colors.dart';

class MedicalRecord extends StatefulWidget {
  const MedicalRecord({super.key});

  @override
  State<MedicalRecord> createState() => _MedicalRecordState();
}

class _MedicalRecordState extends State<MedicalRecord> {
  final firstNameController = TextEditingController();
  final middleNameController = TextEditingController();
  final lastNameController = TextEditingController();
  final birthdayController = TextEditingController();
  final ageController = TextEditingController();
  final sexController = TextEditingController();
  final civilStatusController = TextEditingController();
  final addressController = TextEditingController();
  final contactController = TextEditingController();
  final bloodTypeController = TextEditingController();

  bool asthma = false;
  bool epilepsy = false;
  bool heartDisease = false;
  bool allergies = false;
  bool othersMedical = false;

  final lastAsthmaController = TextEditingController();
  final lastEpilepsyController = TextEditingController();
  final heartDiseaseController = TextEditingController();
  final medicationsController = TextEditingController();
  final surgeriesController = TextEditingController();
  final allergiesController = TextEditingController();
  final othersMedicalController = TextEditingController();
  final familyHistoryController = TextEditingController();

  final heightController = TextEditingController();
  final weightController = TextEditingController();
  final bmiController = TextEditingController();
  final bpController = TextEditingController();
  final prController = TextEditingController();
  final tempController = TextEditingController();

  final remarksController = TextEditingController();
  final recommendationsController = TextEditingController();
  final patientSignController = TextEditingController();
  final officerSignController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF5F7FA),
      appBar: AppBar(
        automaticallyImplyLeading: false,
        title: const Text(
          "Medical Record",
          style: TextStyle(fontWeight: FontWeight.w700, fontSize: 18),
        ),
        flexibleSpace: Container(
          decoration: const BoxDecoration(gradient: AppColors.primaryGradient),
        ),
        foregroundColor: Colors.white,
        elevation: 0,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16),
        child: Column(
          children: [
            _buildSection(
              title: "Personal Information",
              icon: Icons.person_outline,
              children: [
                _buildUniformGrid([
                  firstNameController,
                  middleNameController,
                  lastNameController,
                  birthdayController,
                  ageController,
                  sexController,
                  civilStatusController,
                  bloodTypeController,
                  addressController,
                  contactController,
                ], [
                  'First Name',
                  'Middle Name',
                  'Last Name',
                  'Birthday',
                  'Age',
                  'Sex',
                  'Civil Status',
                  'Blood Type',
                  'Address',
                  'Contact Number',
                ]),
              ],
            ),
            const SizedBox(height: 16),
            _buildSection(
              title: "Medical History",
              icon: Icons.medical_services_outlined,
              children: [
                _buildMedicalConditions(),
                const SizedBox(height: 16),
                _buildUniformGrid([
                  if (asthma) lastAsthmaController,
                  if (epilepsy) lastEpilepsyController,
                  if (heartDisease) heartDiseaseController,
                  if (allergies) allergiesController,
                  if (othersMedical) othersMedicalController,
                  medicationsController,
                  surgeriesController,
                  familyHistoryController,
                ], [
                  if (asthma) 'Last Asthma Attack',
                  if (epilepsy) 'Last Epilepsy Episode',
                  if (heartDisease) 'Heart Disease Details',
                  if (allergies) 'Specify Allergies',
                  if (othersMedical) 'Other Medical Conditions',
                  'Current Medications',
                  'Past Surgeries / Hospitalization',
                  'Family Medical History',
                ]),
              ],
            ),
            const SizedBox(height: 16),
            _buildSection(
              title: "Physical Examination",
              icon: Icons.monitor_heart_outlined,
              children: [
                _buildUniformGrid([
                  heightController,
                  weightController,
                  bmiController,
                  bpController,
                  prController,
                  tempController,
                ], [
                  'Height (cm)',
                  'Weight (kg)',
                  'BMI',
                  'Blood Pressure',
                  'Pulse Rate',
                  'Temperature (°C)',
                ]),
              ],
            ),
            const SizedBox(height: 16),
            _buildSection(
              title: "Assessment & Signature",
              icon: Icons.assignment_outlined,
              children: [
                _buildUniformGrid([
                  remarksController,
                  recommendationsController,
                  patientSignController,
                  officerSignController,
                ], [
                  'Medical Remarks',
                  'Recommendations',
                  "Patient's Signature",
                  'Medical Officer / Nurse',
                ]),
              ],
            ),
            const SizedBox(height: 24),
            _buildSubmitButton(),
            const SizedBox(height: 24),
          ],
        ),
      ),
    );
  }

  Widget _buildMedicalConditions() {
    return Container(
      decoration: BoxDecoration(
        gradient: LinearGradient(
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
          colors: [
            AppColors.primaryGradient.colors.first.withOpacity(0.05),
            AppColors.primaryGradient.colors.last.withOpacity(0.08),
          ],
        ),
        borderRadius: BorderRadius.circular(14),
        border: Border.all(
          color: AppColors.primaryGradient.colors.first.withOpacity(0.15),
          width: 1.5,
        ),
      ),
      padding: const EdgeInsets.all(16),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Container(
                padding: const EdgeInsets.all(8),
                decoration: BoxDecoration(
                  gradient: AppColors.primaryGradient,
                  borderRadius: BorderRadius.circular(8),
                  boxShadow: [
                    BoxShadow(
                      color: AppColors.primaryGradient.colors.first.withOpacity(0.3),
                      blurRadius: 8,
                      offset: const Offset(0, 2),
                    ),
                  ],
                ),
                child: const Icon(
                  Icons.health_and_safety_outlined,
                  color: Colors.white,
                  size: 18,
                ),
              ),
              const SizedBox(width: 10),
              const Text(
                'Medical Conditions',
                style: TextStyle(
                  fontSize: 14,
                  fontWeight: FontWeight.w700,
                  letterSpacing: -0.2,
                ),
              ),
              const Spacer(),
              Container(
                padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 4),
                decoration: BoxDecoration(
                  color: Colors.white.withOpacity(0.7),
                  borderRadius: BorderRadius.circular(12),
                ),
                child: Text(
                  'Select all that apply',
                  style: TextStyle(
                    fontSize: 11,
                    fontWeight: FontWeight.w500,
                    color: Colors.grey.shade600,
                  ),
                ),
              ),
            ],
          ),
          const SizedBox(height: 14),
          Wrap(
            spacing: 10,
            runSpacing: 10,
            children: [
              _buildConditionCard(
                'Asthma',
                Icons.air,
                asthma,
                (val) => setState(() => asthma = val),
              ),
              _buildConditionCard(
                'Epilepsy',
                Icons.psychology_outlined,
                epilepsy,
                (val) => setState(() => epilepsy = val),
              ),
              _buildConditionCard(
                'Heart Disease',
                Icons.favorite_outline,
                heartDisease,
                (val) => setState(() => heartDisease = val),
              ),
              _buildConditionCard(
                'Allergies',
                Icons.healing_outlined,
                allergies,
                (val) => setState(() => allergies = val),
              ),
              _buildConditionCard(
                'Others',
                Icons.more_horiz,
                othersMedical,
                (val) => setState(() => othersMedical = val),
              ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildConditionCard(
    String label,
    IconData icon,
    bool value,
    Function(bool) onChanged,
  ) {
    return InkWell(
      onTap: () => onChanged(!value),
      borderRadius: BorderRadius.circular(12),
      child: AnimatedContainer(
        duration: const Duration(milliseconds: 250),
        curve: Curves.easeInOut,
        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 10),
        decoration: BoxDecoration(
          gradient: value ? AppColors.primaryGradient : null,
          color: value ? null : Colors.white,
          borderRadius: BorderRadius.circular(12),
          border: Border.all(
            color: value
                ? Colors.transparent
                : Colors.grey.shade300,
            width: 1.5,
          ),
          boxShadow: value
              ? [
                  BoxShadow(
                    color: AppColors.primaryGradient.colors.first.withOpacity(0.4),
                    blurRadius: 8,
                    offset: const Offset(0, 4),
                  )
                ]
              : [
                  BoxShadow(
                    color: Colors.black.withOpacity(0.03),
                    blurRadius: 4,
                    offset: const Offset(0, 2),
                  )
                ],
        ),
        child: Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            Icon(
              icon,
              size: 18,
              color: value ? Colors.white : Colors.grey.shade600,
            ),
            const SizedBox(width: 8),
            Text(
              label,
              style: TextStyle(
                fontSize: 13,
                fontWeight: FontWeight.w600,
                color: value ? Colors.white : Colors.grey.shade700,
              ),
            ),
            const SizedBox(width: 6),
            AnimatedContainer(
              duration: const Duration(milliseconds: 250),
              width: 18,
              height: 18,
              decoration: BoxDecoration(
                color: value ? Colors.white.withOpacity(0.3) : Colors.grey.shade200,
                shape: BoxShape.circle,
              ),
              child: value
                  ? const Icon(Icons.check, size: 12, color: Colors.white)
                  : null,
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildUniformGrid(
    List<TextEditingController> controllers,
    List<String> labels,
  ) {
    return LayoutBuilder(
      builder: (context, constraints) {
        final crossAxisCount = constraints.maxWidth > 600 ? 2 : 2;
        final itemWidth =
            (constraints.maxWidth - ((crossAxisCount - 1) * 10)) / crossAxisCount;

        return Wrap(
          spacing: 10,
          runSpacing: 12,
          children: List.generate(controllers.length, (index) {
            return SizedBox(
              width: itemWidth,
              child: _animatedTextField(
                controllers[index],
                labels[index],
              ),
            );
          }),
        );
      },
    );
  }

  Widget _animatedTextField(
    TextEditingController controller,
    String label, {
    TextInputType keyboard = TextInputType.text,
    int maxLines = 1,
  }) {
    final focusNode = FocusNode();
    return StatefulBuilder(
      builder: (context, setState) {
        focusNode.addListener(() => setState(() {}));
        return AnimatedContainer(
          duration: const Duration(milliseconds: 200),
          decoration: BoxDecoration(
            gradient: focusNode.hasFocus ? AppColors.primaryGradient : null,
            borderRadius: BorderRadius.circular(10),
            boxShadow: focusNode.hasFocus
                ? [
                    BoxShadow(
                      color: AppColors.primaryGradient.colors.first.withOpacity(0.15),
                      blurRadius: 8,
                      offset: const Offset(0, 2),
                    )
                  ]
                : [],
          ),
          padding: EdgeInsets.all(focusNode.hasFocus ? 1.5 : 0),
          child: Container(
            decoration: BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.circular(10),
              border: focusNode.hasFocus
                  ? null
                  : Border.all(color: Colors.grey.shade200),
            ),
            child: TextFormField(
              focusNode: focusNode,
              controller: controller,
              keyboardType: keyboard,
              maxLines: maxLines,
              style: const TextStyle(fontSize: 13, fontWeight: FontWeight.w500),
              decoration: InputDecoration(
                labelText: label,
                labelStyle: TextStyle(
                  color: Colors.grey.shade600,
                  fontSize: 12,
                  fontWeight: FontWeight.w500,
                ),
                border: InputBorder.none,
                contentPadding: const EdgeInsets.symmetric(
                  vertical: 12,
                  horizontal: 12,
                ),
                isDense: true,
              ),
            ),
          ),
        );
      },
    );
  }

  Widget _buildSection({
    required String title,
    required IconData icon,
    required List<Widget> children,
  }) {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(16),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withOpacity(0.04),
            blurRadius: 10,
            offset: const Offset(0, 2),
          ),
        ],
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Container(
                padding: const EdgeInsets.all(10),
                decoration: BoxDecoration(
                  gradient: AppColors.primaryGradient,
                  borderRadius: BorderRadius.circular(12),
                ),
                child: Icon(icon, color: Colors.white, size: 20),
              ),
              const SizedBox(width: 12),
              Text(
                title,
                style: const TextStyle(
                  fontSize: 16,
                  fontWeight: FontWeight.w700,
                  letterSpacing: -0.3,
                ),
              ),
            ],
          ),
          const SizedBox(height: 16),
          ...children,
        ],
      ),
    );
  }

  Widget _buildSubmitButton() {
    return Material(
      color: Colors.transparent,
      child: InkWell(
        onTap: () => debugPrint("Download PDF..."),
        borderRadius: BorderRadius.circular(14),
        child: Ink(
          decoration: BoxDecoration(
            gradient: AppColors.primaryGradient,
            borderRadius: BorderRadius.circular(14),
            boxShadow: [
              BoxShadow(
                color: AppColors.primaryGradient.colors.first.withOpacity(0.3),
                offset: const Offset(0, 4),
                blurRadius: 12,
              )
            ],
          ),
          child: Container(
            width: double.infinity,
            padding: const EdgeInsets.symmetric(vertical: 16),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: const [
                Icon(Icons.download_rounded, color: Colors.white, size: 20),
                SizedBox(width: 8),
                Text(
                  "Download PDF",
                  style: TextStyle(
                    color: Colors.white,
                    fontWeight: FontWeight.w700,
                    fontSize: 15,
                    letterSpacing: 0.3,
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}