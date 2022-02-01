var UsersDirectlyId = "";
var UsersFunctionalId = "";
var sendDirection = 2;
$(document).ready(function () {

    $("#saveForm").click(function () {
        sendDirection = 1
        $("#gAnalize").click();
        sendDirection = 2

    });
    
    $("#gAnalize").click(function () {
        var formData = new FormData();
        var FunkPost = [];
        var EquipmentPost = [];
        var EducationPost = [];
        var CertificatePost = [];
        var AdvantagePost = [];
        var PositionSkillPost = [];
        var LanguagePost = [];
        var ComputerPost =[];
        var PersonQualityPost = [];
        var IndividualSkillPost = [];
        var LeadershipSkillPost = [];
        var ResponsibilityPost = [];
        var PowerPost = [];
        var UsersReplacePost = [];
        var ReportPost = [];
        var documentName = [];
       
        var StartDate = $("#StartDate").val();
         
        var ToolId = $(".tolls option:selected").val();
        var AnalizTipId = $("#analizTip").children("option:selected").val();
        if (AnalizTipId == "" || AnalizTipId == undefined) {
            AnalizTipId = $("#analizTip option:selected").val();
            StartDate = $("#StartDate").attr("value");
        }
        var userId = $("#userId").val();
        var IsOnline = $(".isOnline option:selected").val();
        var CityId = $("#placeTextBox").children("option:selected").val();
        var EndDate = $("#EndDate").val();
        var VeziveninMeqsed = $("#VeziveninMeqsed").val();
        var TimeModeId = $(".TimeMode option:selected").val();
        var TimeModeNote = $(".TimeModeNote").val();
        var IsPhysical = $(".IsPhysical option:selected").val();
        var IsPhysicalNote = $(".IsPhysicalNote").val();
        var PlaceId = $(".Place option:selected").val();
        var PlaceNote = $(".PlaceNote").val();
        var IsWeight = $(".IsWeight option:selected").val();
        var IsWeightNote = $(".IsWeightNote").val();
        var IsWalk = $(".IsWeight option:selected").val();
        var IsWalkNote = $(".IsWeightNote").val();
        if (StartDate == "undefined" || StartDate.length == 0) {
            StartDate = $("#start").val();
        }
        var funk = { 'Funk': '', 'Time': '', 'Result': '', 'Icra': '', 'Defe': '', 'Note': '','IsPeriodik':'' }
        $(".funcBodyText").each(function () {
            funk.Funk = $('.funk', this).text();
            funk.Icra = $('.icra', this).text();
            funk.Result = $('.result', this).text();
            funk.Time = $('.time', this).text();
            funk.Defe = $('.defe', this).text();
            funk.Note = $('.noteFunction', this).text();
            funk.IsPeriodik = $('.periodik', this).text().trim();
            FunkPost.push(funk);
            funk = { 'Funk': '', 'Time': '', 'Result': '', 'Icra': '', 'Defe': '', 'Note': '', 'IsPeriodik': ''}
        });
        var Equipment = { 'EquipmentName': '', 'EquipmentTimeSpanId': ''  }
        $("#EquipmentTableBody tr").each(function () {
           
            Equipment.EquipmentName = $('.equipmentName', this).text();
            Equipment.EquipmentTimeSpanId = $('.equipmentTimeSpan', this).attr("data-timeId");
            EquipmentPost.push(Equipment);
            Equipment = { 'EquipmentName': '', 'EquipmentTimeSpanId': '' }
        });

        var Education = { 'EduDegreeNameId': '', 'EduLevelNameId': '','EduDirection':'' }
        $("#EducationTableBody tr").each(function () {
            Education.EduDegreeNameId = $('.eduDegreeName', this).attr("data-eduDegreeId");
            Education.EduLevelNameId = $('.eduLevelName', this).attr("data-eduLevelId");
            Education.EduDirection = $('.eduDirection', this).text();
            EducationPost.push(Education);
            Education = { 'EduDegreeNameId': '', 'EduLevelNameId': '', 'EduDirection': '' }
        });


        var Certificate = { 'CertificateName': '' }
        $("#CertificateTableBody tr").each(function () {
            Certificate.CertificateName = $('.certificateName', this).text();
            

            CertificatePost.push(Certificate);
            Certificate = { 'CertificateName': '' }
        });
        var Advantage = { 'AdvantageName': '' }
        $("#AdvantageTableBody tr").each(function () {
            Advantage.AdvantageName = $('.advantageName', this).text();
            AdvantagePost.push(Advantage);
            Advantage = { 'AdvantageName': '' }
        });

        var experienceGeneralYear = $(".experienceGeneralYear").val();
        var experienceGeneralMonth = $(".experienceGeneralMonth").val();
        var experienceLeaderYear = $(".experienceLeaderYear").val();
        var experienceLeaderMonth = $(".experienceLeaderMonth").val();
        var otherRequirement = $(".otherRequirement").val();
          UsersDirectlyId = $("#UsersDirectly option:selected").val();
          UsersFunctionalId = $("#UsersFunctional option:selected").val();

        var PositionSkill = { 'PositionSkill': '', 'PositionSkilldegree': '' }
        $("#PositionSkillTableBody tr").each(function () {
            PositionSkill.PositionSkill = $('.positionSkill', this).text();
            PositionSkill.PositionSkilldegree = $('.positionSkilldegree', this).text();
            PositionSkillPost.push(PositionSkill);
            PositionSkill = { 'PositionSkill': '', 'PositionSkilldegree': '' }
        });


        var Language = { 'LanguageName': '', 'LanguageLevelId': '' }
        $("#LanguageTableBody tr").each(function () {
            Language.LanguageName = $('.languageName', this).text();
            Language.LanguageLevelId = $('.languageLevel', this).attr("data-LangLevelId");
            LanguagePost.push(Language);
            Language = { 'LanguageName': '', 'LanguageLevelId': '' }
        });

        var Computer = { 'ComputerName': '', 'ComputerLevelId': '' }
        $("#ComputerTableBody tr").each(function () {
            Computer.ComputerName = $('.computerName', this).text();
            Computer.ComputerLevelId = $('.computerLevel', this).attr("data-ComputerLevelId");
            ComputerPost.push(Computer);
            Computer = { 'ComputerName': '', 'ComputerLevelId': '' }
        });
        var PersonQuality = { 'PersonQuality': '' }
        $("#PersonQualityTableBody tr").each(function () {
            PersonQuality.PersonQuality = $('.personQuality', this).text();
            PersonQualityPost.push(PersonQuality);
            PersonQuality = { 'PersonQuality': '' }
        });

        var IndividualSkill = { 'IndividualSkill': '' }
        $("#IndividualSkillTableBody tr").each(function () {
            IndividualSkill.IndividualSkill = $('.individualSkill', this).text();
            IndividualSkillPost.push(IndividualSkill);
            IndividualSkill = { 'IndividualSkill': '' }
        });
        var LeadershipSkill = { 'LeadershipSkill': '' }
        $("#LeadershipSkillTableBody tr").each(function () {
            LeadershipSkill.LeadershipSkill = $('.leadershipSkill', this).text();
            LeadershipSkillPost.push(LeadershipSkill);
            LeadershipSkill = { 'LeadershipSkill': '' }
        });
      

        var Responsibility = { 'ResponsibilityDescription': '', 'ResponsibilityTypeId': '', 'ResponsibilityCommitmentId': '','ResponsibilityRelationId':'', 'ResponsibilityEffectId':'' }
        $("#ResponsibilityTableBody tr").each(function () {
            Responsibility.ResponsibilityDescription = $('.ResponsibilityDescription', this).text();
            Responsibility.ResponsibilityTypeId = $('.responsibilityType', this).attr("data-ResponsibilityTypeId");
            Responsibility.ResponsibilityCommitmentId = $('.responsibilityCommitment', this).attr("data-ResponsibilityCommitmentId");
            Responsibility.ResponsibilityRelationId = $('.responsibilityRelation', this).attr("data-ResponsibilityRelationId");
            Responsibility.ResponsibilityEffectId = $('.responsibilityEffect', this).attr("data-ResponsibilityEffectId");
            ResponsibilityPost.push(Responsibility);
            Responsibility = { 'ResponsibilityDescription': '', 'ResponsibilityTypeId': '', 'ResponsibilityCommitmentId': '', 'ResponsibilityRelationId': '', 'ResponsibilityEffectId': '' }
        });
        var Power = { 'PowersName': '' }
        $("#PowersTableBody tr").each(function () {
            Power.PowersName = $('.powersName', this).text();
            PowerPost.push(Power);
            Power = { 'PowersName': '' }
        });
       

        var UsersReplace = { 'UsersReplaceNameId': '' }
        $("#UsersReplaceTableBody tr").each(function () {
            UsersReplace.UsersReplaceNameId = $('.usersReplaceName', this).attr("data-UserReplaceId");
            UsersReplacePost.push(UsersReplace);
            UsersReplace = { 'UsersReplaceNameId': '' }
        });
        



        var hesabatTable = { 'HesabName': '', 'HesabatType': '', 'HesabPersonName': '', 'PeriodHes': '','HesabNote':'', "HesabatIdPerson": '' }
        $("#hesabatTableBody tr").each(function () {
            hesabatTable.HesabName = $('.hesabName', this).text();
            hesabatTable.HesabatType = $('.hesabType', this).text();
            hesabatTable.HesabPersonName = $('.hesabPersonName', this).text();
            hesabatTable.PeriodHes = $('.periodHes', this).text();
            hesabatTable.HesabNote = $('.NoteReport', this).text();
            hesabatTable.HesabatIdPerson = $('.userIdHesab', this).text();
            ReportPost.push(hesabatTable);
            hesabatTable = { 'HesabName': '', 'HesabatType': '', 'HesabPersonName': '', 'PeriodHes': '', 'HesabNote': '',"HesabatIdPerson":'' }
        });



       
        $("#senedBody tr").each(function () {
            var sen = $('.senedInput', this).val()
            if (sen != undefined) {
                documentName.push(sen);
            }
        })
        if (sendDirection == 2) {
            //validation
            if (userId.length == 0) {
                alert("Sualları cavablandıran qeyd olunmayıb!")
                return;
            } else if (StartDate.length == 0) {
                alert("Başlama tarixi qeyd olunmayıb!")
                return;
            } else if (EndDate.length == 0) {
                alert("Bitmə tarixi qeyd olunmayıb!")
                return;
            } else if (VeziveninMeqsed.length == 0) {
                alert("Vəzifənin məqsədi qeyd olunmayıb!")
                return;
            } else if (FunkPost.length == 0) {
                alert("İcra edilən funksiyalar haqqinda məlumat qeyd olunmayıb!")
                return;
            } else if (EducationPost.length == 0) {
                alert(" Təhsil üzrə tələblər qeyd olunmayıb!")
                return;
            } else if (CertificatePost.length == 0) {
                alert(" Sertifikat, vəsiqə, lisenziya, şəhadətnamə və s. qeyd olunmayıb!")
                return;
            } else if (experienceGeneralYear.length == 0 && experienceGeneralMonth.length == 0 && experienceLeaderMonth.length == 0 && experienceLeaderYear.length == 0) {
                alert("Təcrübə üzrə tələblər qeyd olunmayıb!")
                return;
            } else if (PositionSkillPost.length == 0) {
                alert("Vəzifə üzrə funksional bilik və bacarıqlar qeyd olunmayıb!")
                return;
            } else if (LanguagePost.length == 0) {
                alert("Dil bilikləri qeyd olunmayıb!")
                return;
            } else if (ComputerPost.length == 0) {
                alert("Rəqəmsal biliklər qeyd olunmayıb!")
                return;
            } else if (PersonQualityPost.length == 0) {
                alert("Ümumi fərdi keyfiyyətlər qeyd olunmayıb!")
                return;
            } else if (IndividualSkillPost.length == 0) {
                alert("Fərdi keyfiyyət bacarıqları qeyd olunmayıb!")
                return;
            } else if (LeadershipSkillPost.length == 0) {
                alert("Rəhbərlik üzrə bacarıqlar qeyd olunmayıb!")
                return;
            } else if (ResponsibilityPost.length == 0) {
                alert("Cavabdehliyin təsviri qeyd olunmayıb!")
                return;
            } else if (PowerPost.length == 0) {
                alert("Səlahiyyətlər qeyd olunmayıb!")
                return;
            } else if (UsersDirectlyId == undefined || UsersDirectlyId == "") {
                alert("Birbaşa kimə tabedir qeyd olunmayıb!")
                return;
            } else if (UsersFunctionalId == undefined || UsersFunctionalId == "") {
                alert("Funksional kimə tabedir qeyd olunmayıb!")
                return;
            } else if (documentName == undefined || documentName == "") {
                alert("Əlaqəli sənəd qeyd olunmayıb!")
                return;
            } else if ($(".ishaveReport option:selected").val() == "true") {
                if (ReportPost.length == 0 || ReportPost == undefined) {
                    alert("Hesabatlar barədə məlumat qeyd olunmayıb!")
                    return;
                }
            }
        } else {
            if (userId.length == 0) {
                alert("Sualları cavablandıran qeyd olunmayıb!")
                return;
            }  
            else if (UsersDirectlyId == undefined || UsersDirectlyId == "") {
                alert("Birbaşa kimə tabedir qeyd olunmayıb!")
                return;
            } else if (UsersFunctionalId == undefined || UsersFunctionalId == "") {
                alert("Funksional kimə tabedir qeyd olunmayıb!")
                return;
            }
        }
        
         

        formData.append('FunkPost', JSON.stringify(FunkPost))
        formData.append('EquipmentPost', JSON.stringify(EquipmentPost))
        formData.append('EducationPost', JSON.stringify(EducationPost))
        formData.append('CertificatePost', JSON.stringify(CertificatePost))
        formData.append('AdvantagePost', JSON.stringify(AdvantagePost))
        formData.append('PositionSkillPost', JSON.stringify(PositionSkillPost ))
        formData.append('LanguagePost', JSON.stringify(LanguagePost))
        formData.append('ComputerPost', JSON.stringify(ComputerPost))
        formData.append('PersonQualityPost', JSON.stringify(PersonQualityPost))
        formData.append('IndividualSkillPost', JSON.stringify(IndividualSkillPost))
        formData.append('ResponsibilityPost', JSON.stringify(ResponsibilityPost))
        formData.append('PowerPost', JSON.stringify(PowerPost))
        formData.append('UsersReplacePost', JSON.stringify(UsersReplacePost))
        formData.append('ReportPost', JSON.stringify(ReportPost)) 
        formData.append('LeadershipSkillPost', JSON.stringify(LeadershipSkillPost))
        formData.append('documentName', documentName)
        formData.append('IsOnline', IsOnline)
        formData.append('AnalizTipId', AnalizTipId)
        formData.append('userId', userId)
        formData.append('StartDate', StartDate)
        formData.append('CityId', CityId)
        formData.append('EndDate', EndDate)
        formData.append('VeziveninMeqsed', VeziveninMeqsed)
        formData.append('ToolId', ToolId)
        formData.append('TimeModeId', TimeModeId)
        formData.append('TimeModeNote', TimeModeNote)
        formData.append('IsPhysical', IsPhysical)
        formData.append('IsPhysicalNote', IsPhysicalNote)
        formData.append('PlaceId', PlaceId)
        formData.append('PlaceNote', PlaceNote)
        formData.append('IsWeight', IsWeight)
        formData.append('IsWeightNote', IsWeightNote)
        formData.append('IsWalk', IsWalk )
        formData.append('IsWalkNote', IsWalkNote)
        formData.append('UsersDirectlyId', UsersDirectlyId)
        formData.append('UsersFunctionalId', UsersFunctionalId)
        formData.append('experienceGeneralYear', experienceGeneralYear)
        formData.append('experienceGeneralMonth', experienceGeneralMonth)
        formData.append('experienceLeaderYear', experienceLeaderYear)
        formData.append('experienceLeaderMonth', experienceLeaderMonth)
        formData.append('otherRequirement', otherRequirement)
        formData.append('sendDirection', sendDirection)

        //get value end
            $.ajax({
                type: 'POST',
                url: '/Analiz/Create',
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.status === "success") {

                        alert("Anketdə iştirak etdiyiniz üçün təşəkkür edirik!")
                        window.location.reload();
                    }
                    else {
                        alert("Xəta baş verdi,şəbəkəni yoxluyun!")
                        $("#gAnalize").show()
                    }
                }
            });
    })
});

    