import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';
import '../widgets/app_colors.dart';
import '../mock/mock_patient_record_db.dart';

class PatientRecords extends StatefulWidget {
  const PatientRecords({super.key});

  @override
  State<PatientRecords> createState() => _PatientRecordsState();
}

class PatientRecord {
  final String recordID;
  final String date;
  final String diagnosis;
  final String symptoms;
  final String treatment;
  final String notes;

  PatientRecord({
    required this.recordID,
    required this.date,
    required this.diagnosis,
    required this.symptoms,
    required this.treatment,
    this.notes = '',
  });
}

class _PatientRecordsState extends State<PatientRecords> {
  final _formKey = GlobalKey<FormState>();
  final _recordIDController = TextEditingController();
  final _dateController = TextEditingController();
  final _diagnosisController = TextEditingController();
  final _treatmentController = TextEditingController();
  final _notesController = TextEditingController();
  final _otherSymptomController = TextEditingController();

  int _lastRecordNumber = 0;
  final _db = MockPatientRecordDb();

  final List<Map<String, dynamic>> _symptoms = [
    {"name": "Fever", "icon": Icons.thermostat_rounded},
    {"name": "Cough", "icon": Icons.sick_rounded},
    {"name": "Dizziness", "icon": Icons.rotate_right_rounded},
    {"name": "Chest Pain", "icon": Icons.monitor_heart_rounded},
    {"name": "Fatigue", "icon": Icons.battery_full_rounded},
    {"name": "Injury", "icon": Icons.healing_rounded},
    {"name": "Other:", "icon": Icons.edit_note_rounded},
  ];
  final List<String> _selectedSymptoms = [];

  @override
  void initState() {
    super.initState();
    _recordIDController.text = _generateRecordID();
  }

  @override
  void dispose() {
    for (final c in [
      _recordIDController,
      _dateController,
      _diagnosisController,
      _treatmentController,
      _notesController,
      _otherSymptomController,
    ]) {
      c.dispose();
    }
    super.dispose();
  }

  String _generateRecordID() {
    _lastRecordNumber++;
    return _lastRecordNumber.toString().padLeft(3, '0');
  }

  Future<void> _pickDate() async {
    final picked = await showDatePicker(
      context: context,
      initialDate: DateTime.now(),
      firstDate: DateTime(2000),
      lastDate: DateTime.now().add(const Duration(days: 365 * 2)),
      builder: (context, child) => Theme(
        data: ThemeData.light().copyWith(
          colorScheme: ColorScheme.light(
            primary: AppColors.primaryGradient.colors.first,
          ),
        ),
        child: child!,
      ),
    );
    if (picked != null) {
      _dateController.text = DateFormat('MMMM dd, yyyy').format(picked);
    }
  }

  void _submitForm() {
    if (_formKey.currentState?.validate() ?? false) {
      if (_selectedSymptoms.contains("Other:") &&
          _otherSymptomController.text.isNotEmpty) {
        _selectedSymptoms.remove("Other:");
        _selectedSymptoms.add(_otherSymptomController.text);
      }

      final record = PatientRecord(
        recordID: 'R${_recordIDController.text}',
        date: _dateController.text,
        diagnosis: _diagnosisController.text,
        symptoms: _selectedSymptoms.join(", "),
        treatment: _treatmentController.text,
        notes: _notesController.text,
      );

      _db.addRecord(record);
      _db.printAllRecords();

      _recordIDController.text = _generateRecordID();
      _dateController.clear();
      _diagnosisController.clear();
      _treatmentController.clear();
      _notesController.clear();
      _selectedSymptoms.clear();
      _otherSymptomController.clear();

      showDialog(
        context: context,
        builder: (context) => Dialog(
          shape:
              RoundedRectangleBorder(borderRadius: BorderRadius.circular(20)),
          child: Padding(
            padding: const EdgeInsets.all(24),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                Container(
                  width: 80,
                  height: 80,
                  decoration: const BoxDecoration(
                    gradient: AppColors.primaryGradient,
                    shape: BoxShape.circle,
                  ),
                  child: const Icon(Icons.done_all_rounded,
                      color: Colors.white, size: 50),
                ),
                const SizedBox(height: 20),
                const Text(
                  "Form Submitted!",
                  style: TextStyle(
                    fontSize: 22,
                    fontWeight: FontWeight.w700,
                    color: Color.fromARGB(221, 24, 129, 20),
                  ),
                  textAlign: TextAlign.center,
                ),
                const SizedBox(height: 12),
                RichText(
                  textAlign: TextAlign.center,
                  text: TextSpan(
                    children: [
                      TextSpan(
                          text: "Patient ID: ${record.recordID}\n",
                          style: const TextStyle(
                              fontSize: 18,
                              fontWeight: FontWeight.bold,
                              color: Colors.black)),
                      TextSpan(
                          text: record.date,
                          style: const TextStyle(
                              fontSize: 16,
                              fontWeight: FontWeight.w500,
                              color: Colors.grey)),
                    ],
                  ),
                ),
              ],
            ),
          ),
        ),
      );
    }
  }

  Widget _animatedTextField(
    TextEditingController controller,
    String label,
    IconData icon, {
    TextInputType keyboard = TextInputType.text,
    bool readOnly = false,
    bool requiredField = true,
    int maxLines = 1,
    VoidCallback? onTap,
  }) {
    final focusNode = FocusNode();
    return StatefulBuilder(
      builder: (context, setState) {
        focusNode.addListener(() => setState(() {}));
        return AnimatedContainer(
          duration: const Duration(milliseconds: 250),
          decoration: BoxDecoration(
            gradient: focusNode.hasFocus
                ? AppColors.primaryGradient
                : const LinearGradient(colors: [Colors.white, Colors.white]),
            borderRadius: BorderRadius.circular(15),
            boxShadow: focusNode.hasFocus
                ? [
                    BoxShadow(
                      color: AppColors.primaryGradient.colors.first
                          .withOpacity(0.25),
                      blurRadius: 12,
                      offset: const Offset(0, 4),
                    ),
                  ]
                : [],
          ),
          padding: const EdgeInsets.all(1.2),
          child: Container(
            decoration: BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.circular(14),
            ),
            child: TextFormField(
              controller: controller,
              focusNode: focusNode,
              keyboardType: keyboard,
              readOnly: readOnly,
              onTap: onTap,
              maxLines: maxLines,
              validator: (value) =>
                  requiredField && (value == null || value.isEmpty)
                      ? "This field is required"
                      : null,
              decoration: InputDecoration(
                labelText: label,
                prefixIcon: ShaderMask(
                  shaderCallback: (bounds) =>
                      AppColors.primaryGradient.createShader(bounds),
                  child: Icon(icon, color: Colors.white),
                ),
                labelStyle: TextStyle(
                    color: Colors.grey.shade600, fontWeight: FontWeight.w500),
                border: InputBorder.none,
                contentPadding:
                    const EdgeInsets.symmetric(vertical: 18, horizontal: 16),
              ),
            ),
          ),
        );
      },
    );
  }

  Widget _buildRecordIDField() {
    return Container(
      margin: const EdgeInsets.symmetric(vertical: 8),
      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 14),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(14),
        boxShadow: [
          BoxShadow(
            color: AppColors.primaryGradient.colors.first.withOpacity(0.15),
            blurRadius: 12,
            offset: const Offset(0, 5),
          ),
        ],
      ),
      child: Row(
        children: [
          ShaderMask(
            shaderCallback: (bounds) =>
                AppColors.primaryGradient.createShader(bounds),
            child: const Icon(Icons.badge_rounded, color: Colors.white),
          ),
          const SizedBox(width: 12),
          const Text('R',
              style: TextStyle(fontSize: 18, fontWeight: FontWeight.w900)),
          const SizedBox(width: 6),
          SizedBox(
            width: 40,
            child: TextFormField(
              controller: _recordIDController,
              keyboardType: TextInputType.number,
              inputFormatters: [
                FilteringTextInputFormatter.digitsOnly,
                LengthLimitingTextInputFormatter(3),
              ],
              textAlign: TextAlign.center,
              style:
                  const TextStyle(fontSize: 18, fontWeight: FontWeight.w900),
              decoration: const InputDecoration(
                  border: UnderlineInputBorder(), isDense: true),
              validator: (value) =>
                  value == null || value.isEmpty ? 'Required' : null,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildSymptomChip(String symptom, IconData icon, bool isSelected) {
    return InkWell(
      onTap: () {
        setState(() {
          isSelected
              ? _selectedSymptoms.remove(symptom)
              : _selectedSymptoms.add(symptom);
        });
      },
      borderRadius: BorderRadius.circular(16),
      child: AnimatedContainer(
        duration: const Duration(milliseconds: 250),
        width: 120,
        height: 40,
        alignment: Alignment.center,
        decoration: BoxDecoration(
          gradient: isSelected ? AppColors.primaryGradient : null,
          color: isSelected ? null : Colors.white,
          borderRadius: BorderRadius.circular(16),
          border: Border.all(
            color: isSelected ? Colors.transparent : Colors.grey.shade300,
          ),
          boxShadow: isSelected
              ? [
                  BoxShadow(
                    color: AppColors.primaryGradient.colors.first
                        .withOpacity(0.3),
                    blurRadius: 10,
                    offset: const Offset(0, 4),
                  )
                ]
              : null,
        ),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Icon(icon,
                color: isSelected
                    ? Colors.white
                    : AppColors.primaryGradient.colors.first,
                size: 20),
            const SizedBox(width: 6),
            Flexible(
              child: Text(
                symptom,
                overflow: TextOverflow.ellipsis,
                textAlign: TextAlign.center,
                style: TextStyle(
                    fontSize: 14,
                    fontWeight: FontWeight.w600,
                    color: isSelected ? Colors.white : Colors.black87),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildSection({
    required String title,
    required IconData icon,
    String? subtitle,
    required List<Widget> children,
  }) {
    return Container(
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(20),
        boxShadow: [
          BoxShadow(
            color: AppColors.primaryGradient.colors.first.withOpacity(0.08),
            blurRadius: 15,
            offset: const Offset(0, 5),
          ),
        ],
      ),
      padding: const EdgeInsets.all(24),
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
                child: Icon(icon, color: Colors.white, size: 24),
              ),
              const SizedBox(width: 15),
              Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(title,
                      style: const TextStyle(
                          fontSize: 18, fontWeight: FontWeight.w700)),
                  if (subtitle != null)
                    Text(subtitle,
                        style: TextStyle(
                            fontSize: 14, color: Colors.grey.shade600)),
                ],
              ),
            ],
          ),
          const SizedBox(height: 16),
          ...children,
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF5F7FA),
      appBar: AppBar(
        automaticallyImplyLeading: false,
        title: const Text("Patient Record Form",
            style: TextStyle(fontWeight: FontWeight.w700, fontSize: 22)),
        flexibleSpace:
            Container(decoration: const BoxDecoration(gradient: AppColors.primaryGradient)),
        foregroundColor: Colors.white,
        elevation: 0,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 25),
        child: Form(
          key: _formKey,
          child: Column(
            children: [
              _buildRecordIDField(),
              const SizedBox(height: 16),
              _animatedTextField(_dateController, "Date", Icons.calendar_today_rounded,
                  readOnly: true, onTap: _pickDate),
              const SizedBox(height: 16),
              _animatedTextField(_diagnosisController, "Diagnosis",
                  Icons.medical_information_rounded),
              const SizedBox(height: 16),
              _buildSection(
              title: "Symptoms",
              icon: Icons.medical_services_rounded,
              subtitle: "Select all that apply",
              children: [
                const SizedBox(height: 6),
                Wrap(
                  spacing: 8,
                  runSpacing: 8,
                  children: _symptoms.map((symptom) {
                    final isSelected = _selectedSymptoms.contains(symptom["name"]);
                    return _buildSymptomChip(
                        symptom["name"], symptom["icon"], isSelected);
                  }).toList(),
                ),
                if (_selectedSymptoms.contains("Other:")) ...[
                  const SizedBox(height: 6),
                  _animatedTextField(
                    _otherSymptomController,
                    "Please specify...",
                    Icons.edit_note_rounded,
                    requiredField: false,
                    maxLines: 1,
                  ),
                ],
              ],
            ),
              const SizedBox(height: 16),
              _animatedTextField(_treatmentController, "Treatment", Icons.healing_rounded),
              const SizedBox(height: 16),
              _animatedTextField(_notesController, "Notes", Icons.note_rounded,
                  maxLines: 3, requiredField: false),
              const SizedBox(height: 35),
              InkWell(
                onTap: _submitForm,
                borderRadius: BorderRadius.circular(18),
                child: Container(
                  width: double.infinity,
                  padding: const EdgeInsets.symmetric(vertical: 18),
                  decoration: BoxDecoration(
                    gradient: AppColors.primaryGradient,
                    borderRadius: BorderRadius.circular(18),
                    boxShadow: [
                      BoxShadow(
                        color: AppColors.primaryGradient.colors.first.withOpacity(0.5),
                        offset: const Offset(0, 8),
                        blurRadius: 20,
                      ),
                    ],
                  ),
                  child: const Center(
                    child: Text("Save Record",
                        style: TextStyle(
                            color: Colors.white,
                            fontWeight: FontWeight.w700,
                            fontSize: 18)),
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
