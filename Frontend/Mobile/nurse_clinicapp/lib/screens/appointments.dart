import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import '../widgets/app_colors.dart';

class Appointments extends StatefulWidget {
  const Appointments({super.key});

  @override
  State<Appointments> createState() => _AppointmentsState();
}

class _AppointmentsState extends State<Appointments> {
  final _formKey = GlobalKey<FormState>();

  final TextEditingController firstNameController = TextEditingController();
  final TextEditingController middleNameController = TextEditingController();
  final TextEditingController lastNameController = TextEditingController();
  final TextEditingController ageController = TextEditingController();
  final TextEditingController contactController = TextEditingController();
  final TextEditingController dateController = TextEditingController();
  final TextEditingController timeController = TextEditingController();
  final TextEditingController otherSymptomController = TextEditingController();

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

  Future<void> _selectDate(BuildContext context) async {
    final picked = await showDatePicker(
      context: context,
      initialDate: DateTime.now(),
      firstDate: DateTime.now(),
      lastDate: DateTime(DateTime.now().year + 2),
      builder: (context, child) {
        return Theme(
          data: ThemeData.light().copyWith(
            colorScheme: ColorScheme.light(
              primary: AppColors.primaryGradient.colors.first,
            ),
          ),
          child: child!,
        );
      },
    );
    if (picked != null) {
      setState(() {
        dateController.text = DateFormat('MMMM dd, yyyy').format(picked);
      });
    }
  }

  Future<void> _selectTime(BuildContext context) async {
    final picked = await showTimePicker(
      context: context,
      initialTime: TimeOfDay.now(),
      builder: (context, child) {
        return Theme(
          data: ThemeData.light().copyWith(
            colorScheme: ColorScheme.light(
              primary: AppColors.primaryGradient.colors.first,
            ),
          ),
          child: child!,
        );
      },
    );
    if (picked != null) {
      final now = DateTime.now();
      final dt =
          DateTime(now.year, now.month, now.day, picked.hour, picked.minute);
      setState(() {
        timeController.text = DateFormat('h:mm a').format(dt);
      });
    }
  }

  void _submitAppointment() {
    if (_formKey.currentState!.validate()) {
      if (_selectedSymptoms.isEmpty) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: const Text("Please select at least one symptom."),
            backgroundColor: Colors.orange.shade700,
            behavior: SnackBarBehavior.floating,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12),
            ),
          ),
        );
        return;
      }

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
                      color: Color.fromARGB(255, 255, 255, 255), size: 50),
                ),
                const SizedBox(height: 20),
                const Text(
                  "Appointment Submitted!",
                  style: TextStyle(
                    fontSize: 18,
                    fontWeight: FontWeight.w700,
                    color: Color.fromARGB(255, 44, 202, 49),
                  ),
                ),
              ],
            ),
          ),
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF5F7FA),
      appBar: AppBar(
        automaticallyImplyLeading: false,
        title: const Text(
          "Clinic Appointment",
          style: TextStyle(fontWeight: FontWeight.w700, fontSize: 22),
        ),
        flexibleSpace: Container(
          decoration: const BoxDecoration(gradient: AppColors.primaryGradient),
        ),
        foregroundColor: Colors.white,
        elevation: 0,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 25),
        child: Form(
          key: _formKey,
          child: Column(
            children: [
              _buildSection(
                title: "Patient Information",
                icon: Icons.person_rounded,
                children: [
                  _animatedTextField(firstNameController, "First Name",
                      Icons.person_outline_rounded),
                  const SizedBox(height: 16),
                  _animatedTextField(middleNameController, "Middle Name",
                      Icons.person_outline_rounded,
                      required: false),
                  const SizedBox(height: 16),
                  _animatedTextField(lastNameController, "Last Name",
                      Icons.person_outline_rounded),
                  const SizedBox(height: 16),
                  _animatedTextField(ageController, "Age", Icons.cake_rounded,
                      keyboard: TextInputType.number),
                  const SizedBox(height: 16),
                  _animatedTextField(contactController, "Mobile Number",
                      Icons.phone_rounded,
                      keyboard: TextInputType.phone),
                ],
              ),
              const SizedBox(height: 25),
              _buildSection(
                title: "Appointment Schedule",
                icon: Icons.calendar_month_rounded,
                children: [
                  _animatedTextField(dateController, "Select Date",
                      Icons.event_note_rounded,
                      readOnly: true, onTap: () => _selectDate(context)),
                  const SizedBox(height: 16),
                  _animatedTextField(timeController, "Select Time",
                      Icons.access_time_filled,
                      readOnly: true, onTap: () => _selectTime(context)),
                ],
              ),
              const SizedBox(height: 25),
              _buildSection(
                title: "Symptoms",
                icon: Icons.medical_services_rounded,
                subtitle: "Select all that apply",
                children: [
                  const SizedBox(height: 8),
                  Wrap(
                    spacing: 10,
                    runSpacing: 10,
                    children: _symptoms.map((symptom) {
                      final isSelected =
                          _selectedSymptoms.contains(symptom["name"]);
                      return _buildSymptomChip(
                          symptom["name"], symptom["icon"], isSelected);
                    }).toList(),
                  ),
                  if (_selectedSymptoms.contains("Other:")) ...[
                    const SizedBox(height: 16),
                    _animatedTextField(otherSymptomController,
                        "Please specify...", Icons.edit_note_rounded,
                        required: false, maxLines: 3),
                  ],
                ],
              ),
              const SizedBox(height: 35),
              _buildSubmitButton(),
              const SizedBox(height: 30),
            ],
          ),
        ),
      ),
    );
  }

  Widget _animatedTextField(
    TextEditingController controller,
    String label,
    IconData icon, {
    TextInputType keyboard = TextInputType.text,
    bool readOnly = false,
    bool required = true,
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
                    )
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
              focusNode: focusNode,
              controller: controller,
              keyboardType: keyboard,
              readOnly: readOnly,
              onTap: onTap,
              maxLines: maxLines,
              validator: (value) => required && (value == null || value.isEmpty)
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
        padding: const EdgeInsets.symmetric(horizontal: 14, vertical: 12),
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
                    color:
                        AppColors.primaryGradient.colors.first.withOpacity(0.3),
                    blurRadius: 10,
                    offset: const Offset(0, 4),
                  )
                ]
              : null,
        ),
        child: Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            Icon(icon,
                color: isSelected
                    ? Colors.white
                    : AppColors.primaryGradient.colors.first,
                size: 20),
            const SizedBox(width: 8),
            Text(
              symptom,
              style: TextStyle(
                fontSize: 14,
                fontWeight: FontWeight.w600,
                color: isSelected ? Colors.white : Colors.black87,
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
                          fontSize: 18,
                          fontWeight: FontWeight.w700,
                          color: Colors.black87)),
                  if (subtitle != null)
                    Text(
                      subtitle,
                      style: TextStyle(
                          fontSize: 13,
                          color: Colors.grey.shade600,
                          fontWeight: FontWeight.w400),
                    ),
                ],
              ),
            ],
          ),
          const Divider(height: 30, thickness: 1, color: Color(0xFFF5F7FA)),
          ...children,
        ],
      ),
    );
  }

  Widget _buildSubmitButton() {
    return InkWell(
      onTap: _submitAppointment,
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
          child: Text(
            "Submit Appointment",
            style: TextStyle(
              color: Colors.white,
              fontWeight: FontWeight.w700,
              fontSize: 18,
            ),
          ),
        ),
      ),
    );
  }
}
