<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>
  <xsl:template match="acc_custoverrideupd">
    <xsl:choose>
      <xsl:when test="@custid and @minpt_1">
        <table class="tblDesc">
          <tr>
            <td class="colW1">{lbl:CheckPermission}</td>
            <td class="colW2">
              <xsl:value-of select="@checkpermission" />
            </td>
            <td class="colW1">{lbl:UpdMinPos}</td>
            <td class="colW2">
              <xsl:value-of select="@updminpos" />
            </td>

            <td class="colW1">{lbl:UpdMaxPos}</td>
            <td class="colW2">
              <xsl:value-of select="@updmaxpos" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:language}</td>
            <td class="colW2">
              <xsl:value-of select="@language" />
            </td>
           
            <td class="colW1">{lbl:SubAccName}</td>
            <td class="colW2">
              <xsl:value-of select="@subaccname" />
            </td>
            <td class="colW1">{lbl:WriteLog}</td>
            <td class="colW2">
              <xsl:value-of select="@writelog" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:IP}</td>
            <td class="colW2">
              <xsl:value-of select="@ip" />
            </td>

            <td class="colW1">{lbl:ErrCode}</td>
            <td class="colW2">
              <xsl:value-of select="@errcode" />
            </td>
            <td class="colW1">{lbl:ErrMsg}</td>
            <td class="colW2">
              <xsl:value-of select="@errmsg" />
            </td>

          </tr>
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:soccer}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minpt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_1" />
            </td>
            <td class="colW1">{lbl:minpt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_2" />
            </td>
            <td class="colW1">{lbl:minpt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_3" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minpt_4}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_4" />
            </td>
            <td class="colW1">{lbl:minpt_5}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_5" />
            </td>
            <td class="colW1">{lbl:minpt_6}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_6" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minpt_7}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_7" />
            </td>
            <td class="colW1">{lbl:minpt_8}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_8" />
            </td>
            <td class="colW1">{lbl:minpt_9}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_9" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minpt_10}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_10" />
            </td>
            <td class="colW1">{lbl:minlivept_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minlivept_1" />
            </td>
            <td class="colW1">{lbl:minlivept_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minlivept_3" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minlivept_7}</td>
            <td class="colW2">
              <xsl:value-of select="@minlivept_7" />
            </td>
            <td class="colW1">{lbl:minlivept_8}</td>
            <td class="colW2">
              <xsl:value-of select="@minlivept_8" />
            </td>
            <td class="colW1">{lbl:maxpt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_1" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxpt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_2" />
            </td>
            <td class="colW1">{lbl:maxpt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_3" />
            </td>
            <td class="colW1">{lbl:maxpt_4}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_4" />
            </td>
          </tr>
          <tr>

            <td class="colW1">{lbl:maxpt_5}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_5" />
            </td>
            <td class="colW1">{lbl:maxpt_6}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_6" />
            </td>
            <td class="colW1">{lbl:maxpt_7}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_7" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxpt_8}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_8" />
            </td>
            <td class="colW1">{lbl:maxpt_9}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_9" />
            </td>
            <td class="colW1">{lbl:maxpt_10}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_10" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxlivept_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxlivept_1" />
            </td>
            <td class="colW1">{lbl:maxlivept_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxlivept_3" />
            </td>
            <td class="colW1">{lbl:maxlivept_7}</td>
            <td class="colW2">
              <xsl:value-of select="@maxlivept_7" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxlivept_8}</td>
            <td class="colW2">
              <xsl:value-of select="@maxlivept_8" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
          </tr>
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:basketball}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv2pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minv2pt_1" />
            </td>
            <td class="colW1">{lbl:minv2pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@minv2pt_2" />
            </td>
            <td class="colW1">{lbl:minv2pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minv2pt_3" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv2pt_7}</td>
            <td class="colW2">
              <xsl:value-of select="@minv2pt_7" />
            </td>
            <td class="colW1">{lbl:minv2pt_8}</td>
            <td class="colW2">
              <xsl:value-of select="@minv2pt_8" />
            </td>
            <td class="colW1">{lbl:minv2pt_9}</td>
            <td class="colW2">
              <xsl:value-of select="@minv2pt_9" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv2pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@minv2pt_20" />
            </td>
            <td class="colW1">{lbl:maxv2pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv2pt_1" />
            </td>
            <td class="colW1">{lbl:maxv2pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv2pt_2" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv2pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv2pt_3" />
            </td>
            <td class="colW1">{lbl:maxv2pt_7}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv2pt_7" />
            </td>
            <td class="colW1">{lbl:maxv2pt_8}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv2pt_8" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv2pt_9}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv2pt_9" />
            </td>
            <td class="colW1">{lbl:maxv2pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv2pt_20" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
          </tr>
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:football}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv3pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minv3pt_1" />
            </td>
            <td class="colW1">{lbl:minv3pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@minv3pt_2" />
            </td>
            <td class="colW1">{lbl:minv3pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minv3pt_3" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv3pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@minv3pt_20" />
            </td>
            <td class="colW1">{lbl:maxv3pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv3pt_1" />
            </td>
            <td class="colW1">{lbl:maxv3pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv3pt_2" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv3pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv3pt_3" />
            </td>
            <td class="colW1">{lbl:maxv3pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv3pt_20" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
          </tr>
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:tennis}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv5pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minv5pt_1" />
            </td>
            <td class="colW1">{lbl:minv5pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@minv5pt_2" />
            </td>
            <td class="colW1">{lbl:minv5pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minv5pt_3" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv5pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@minv5pt_20" />
            </td>
            <td class="colW1">{lbl:maxv5pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv5pt_1" />
            </td>
            <td class="colW1">{lbl:maxv5pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv5pt_2" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv5pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv5pt_3" />
            </td>
            <td class="colW1">{lbl:maxv5pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv5pt_20" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
          </tr>
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:baseball}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv8pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv8pt_1" />
            </td>
            <td class="colW1">{lbl:minv8pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minv8pt_3" />
            </td>
            <td class="colW1">{lbl:minv8pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@minv8pt_20" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv8pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv8pt_1" />
            </td>
            <td class="colW1">{lbl:maxv8pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv8pt_3" />
            </td>
            <td class="colW1">{lbl:maxv8pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv8pt_20" />
            </td>
          </tr>
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:golf}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv10pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minv10pt_1" />
            </td>
            <td class="colW1">{lbl:minv10pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@minv10pt_2" />
            </td>
            <td class="colW1">{lbl:minv10pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minv10pt_3" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv10pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@minv10pt_20" />
            </td>
            <td class="colW1">{lbl:maxv10pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv10pt_1" />
            </td>
            <td class="colW1">{lbl:maxv10pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv10pt_2" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv10pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv10pt_3" />
            </td>
            <td class="colW1">{lbl:maxv10pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv10pt_20" />
            </td>
            <td class="colW1"></td>
            <td class="colW2"></td>

          </tr>
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:motorsports}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv11pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minv11pt_1" />
            </td>
            <td class="colW1">{lbl:minv11pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@minv11pt_20" />
            </td>
            <td class="colW1">{lbl:maxv11pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv11pt_1" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv11pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv11pt_20" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">
            </td>
            <td class="colW1"></td>
            <td class="colW2">
            </td>
          </tr>
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:othersports}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv99pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minv99pt_1" />
            </td>
            <td class="colW1">{lbl:minv99pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@minv99pt_2" />
            </td>
            <td class="colW1">{lbl:minv99pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minv99pt_3" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv99pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@minv99pt_20" />
            </td>
            <td class="colW1">{lbl:maxv99pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv99pt_1" />
            </td>
            <td class="colW1">{lbl:maxv99pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv99pt_2" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv99pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv99pt_3" />
            </td>
            <td class="colW1">{lbl:maxv99pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv99pt_20" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">
            </td>
          </tr>

          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:MixSportsParlay}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv99pt_9}</td>
            <td class="colW2">
              <xsl:value-of select="@minv99pt_9" />
            </td>
            <td class="colW1">{lbl:maxv99pt_9}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv99pt_9" />
            </td>
            <td class="colW1"></td>
            <td class="colW2"></td>
          </tr>

          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:NumberGame}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv161pt_81}</td>
            <td class="colW2">
              <xsl:value-of select="@minv161pt_81" />
            </td>
            <td class="colW1">{lbl:minv161pt_83}</td>
            <td class="colW2">
              <xsl:value-of select="@minv161pt_83" />
            </td>
            <td class="colW1">{lbl:minv161pt_85}</td>
            <td class="colW2">
              <xsl:value-of select="@minv161pt_85" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv161pt_86}</td>
            <td class="colW2">
              <xsl:value-of select="@minv161pt_86" />
            </td>
            <td class="colW1">{lbl:minv161livept_85}</td>
            <td class="colW2">
              <xsl:value-of select="@minv161livept_85" />
            </td>
            <td class="colW1">{lbl:minv161livept_86}</td>
            <td class="colW2">
              <xsl:value-of select="@minv161livept_86" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv161livept_87}</td>
            <td class="colW2">
              <xsl:value-of select="@minv161livept_87" />
            </td>
            <td class="colW1">{lbl:maxv161pt_81}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv161pt_81" />
            </td>
            <td class="colW1">{lbl:maxv161pt_83}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv161pt_83" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv161pt_85}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv161pt_85" />
            </td>
            <td class="colW1">{lbl:maxv161pt_86}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv161pt_86" />
            </td>
            <td class="colW1">{lbl:maxv161livept_85}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv161livept_85" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv161livept_86}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv161livept_86" />
            </td>
            <td class="colW1">{lbl:maxv161livept_87}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv161livept_87" />
            </td>
            <td class="colW1"></td>
            <td class="colW2"></td>
          </tr>
        </table>
      </xsl:when>
      <xsl:when test="@custid">
        <table class="tblDesc">
          <tr>
            <td class="colW1">{lbl:CheckPermission}</td>
            <td class="colW2">
              <xsl:value-of select="@checkpermission" />
            </td>
            <td class="colW1">{lbl:UpdMinPos}</td>
            <td class="colW2">
              <xsl:value-of select="@updminpos" />
            </td>

            <td class="colW1">{lbl:UpdMaxPos}</td>
            <td class="colW2">
              <xsl:value-of select="@updmaxpos" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:language}</td>
            <td class="colW2">
              <xsl:value-of select="@language" />
            </td>


            <td class="colW1">{lbl:SubAccID}</td>
            <td class="colW2">
              <xsl:value-of select="@subaccid" />
            </td>
            <td class="colW1">{lbl:WriteLog}</td>
            <td class="colW2">
              <xsl:value-of select="@writelog" />
            </td>
          </tr>

          <tr>
            <td class="colW1">{lbl:IP}</td>
            <td class="colW2">
              <xsl:value-of select="@ip" />
            </td>
            <td class="colW1">{lbl:ReturnErrCode}</td>
            <td class="colW2">
              <xsl:value-of select="@returnerrcode" />
            </td>
            <td class="colW1">{lbl:ErrMsg}</td>
            <td class="colW2">
              <xsl:value-of select="@errmsg" />
            </td>
          </tr>
        </table>
      </xsl:when>
      <xsl:when test="@minpt_1">
        <table class="tblDesc">
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:soccer}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minpt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_1" />
            </td>
            <td class="colW1">{lbl:minpt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_2" />
            </td>
            <td class="colW1">{lbl:minpt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_3" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minpt_4}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_4" />
            </td>
            <td class="colW1">{lbl:minpt_5}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_5" />
            </td>
            <td class="colW1">{lbl:minpt_6}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_6" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minpt_7}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_7" />
            </td>
            <td class="colW1">{lbl:minpt_8}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_8" />
            </td>
            <td class="colW1">{lbl:minpt_9}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_9" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minpt_10}</td>
            <td class="colW2">
              <xsl:value-of select="@minpt_10" />
            </td>
            <td class="colW1">{lbl:minlivept_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minlivept_1" />
            </td>
            <td class="colW1">{lbl:minlivept_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minlivept_3" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minlivept_7}</td>
            <td class="colW2">
              <xsl:value-of select="@minlivept_7" />
            </td>
            <td class="colW1">{lbl:minlivept_8}</td>
            <td class="colW2">
              <xsl:value-of select="@minlivept_8" />
            </td>
            <td class="colW1">{lbl:maxpt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_1" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxpt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_2" />
            </td>
            <td class="colW1">{lbl:maxpt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_3" />
            </td>
            <td class="colW1">{lbl:maxpt_4}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_4" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxpt_5}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_5" />
            </td>
            <td class="colW1">{lbl:maxpt_6}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_6" />
            </td>
            <td class="colW1">{lbl:maxpt_7}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_7" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxpt_8}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_8" />
            </td>
            <td class="colW1">{lbl:maxpt_9}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_9" />
            </td>
            <td class="colW1">{lbl:maxpt_10}</td>
            <td class="colW2">
              <xsl:value-of select="@maxpt_10" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxlivept_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxlivept_1" />
            </td>
            <td class="colW1">{lbl:maxlivept_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxlivept_3" />
            </td>
            <td class="colW1">{lbl:maxlivept_7}</td>
            <td class="colW2">
              <xsl:value-of select="@maxlivept_7" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxlivept_8}</td>
            <td class="colW2">
              <xsl:value-of select="@maxlivept_8" />
            </td>

            <td class="colW1"></td>
            <td class="colW2">

            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>

          </tr>
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:basketball}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv2pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minv2pt_1" />
            </td>
            <td class="colW1">{lbl:minv2pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@minv2pt_2" />
            </td>
            <td class="colW1">{lbl:minv2pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minv2pt_3" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv2pt_7}</td>
            <td class="colW2">
              <xsl:value-of select="@minv2pt_7" />
            </td>
            <td class="colW1">{lbl:minv2pt_8}</td>
            <td class="colW2">
              <xsl:value-of select="@minv2pt_8" />
            </td>
            <td class="colW1">{lbl:minv2pt_9}</td>
            <td class="colW2">
              <xsl:value-of select="@minv2pt_9" />
            </td>
            

          </tr>
          <tr>
            <td class="colW1">{lbl:minv2pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@minv2pt_20" />
            </td>
            <td class="colW1">{lbl:maxv2pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv2pt_1" />
            </td>
            <td class="colW1">{lbl:maxv2pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv2pt_2" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv2pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv2pt_3" />
            </td>
            <td class="colW1">{lbl:maxv2pt_7}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv2pt_7" />
            </td>
            <td class="colW1">{lbl:maxv2pt_8}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv2pt_8" />
            </td>
            
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv2pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv2pt_20" />
            </td>
            <td class="colW1"></td>
            <td class="colW2"></td>
            <td class="colW1"></td>
            <td class="colW2"></td>
            
          </tr>
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:football}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv3pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minv3pt_1" />
            </td>
            <td class="colW1">{lbl:minv3pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@minv3pt_2" />
            </td>

            <td class="colW1">{lbl:minv3pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minv3pt_3" />
            </td>
            

          </tr>
          <tr>
            <td class="colW1">{lbl:minv3pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@minv3pt_20" />
            </td>
            <td class="colW1">{lbl:maxv3pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv3pt_1" />
            </td>
            <td class="colW1">{lbl:maxv3pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv3pt_2" />
            </td>
            
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv3pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv3pt_3" />
            </td>
            <td class="colW1">{lbl:maxv3pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv3pt_20" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
          </tr>
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:tennis}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv5pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minv5pt_1" />
            </td>
            <td class="colW1">{lbl:minv5pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@minv5pt_2" />
            </td>
            <td class="colW1">{lbl:minv5pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minv5pt_3" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv5pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@minv5pt_20" />
            </td>
            <td class="colW1">{lbl:maxv5pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv5pt_1" />
            </td>
            <td class="colW1">{lbl:maxv5pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv5pt_2" />
            </td>
            
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv5pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv5pt_3" />
            </td>
            <td class="colW1">{lbl:maxv5pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv5pt_20" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
            
          </tr>
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:baseball}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv8pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minv8pt_1" />
            </td>
            <td class="colW1">{lbl:minv8pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minv8pt_3" />
            </td>
            <td class="colW1">{lbl:minv8pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@minv8pt_20" />
            </td>
            
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv8pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv8pt_1" />
            </td>
            <td class="colW1">{lbl:maxv8pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv8pt_3" />
            </td>
            <td class="colW1">{lbl:maxv8pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv8pt_20" />
            </td>
          </tr>
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:golf}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv10pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minv10pt_1" />
            </td>
            <td class="colW1">{lbl:minv10pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@minv10pt_2" />
            </td>
            <td class="colW1">{lbl:minv10pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minv10pt_3" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv10pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@minv10pt_20" />
            </td>
            <td class="colW1">{lbl:maxv10pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv10pt_1" />
            </td>
            <td class="colW1">{lbl:maxv10pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv10pt_2" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv10pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv10pt_3" />
            </td>
            <td class="colW1">{lbl:maxv10pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv10pt_20" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
          </tr>
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:motorsports}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv11pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minv11pt_1" />
            </td>
            <td class="colW1">{lbl:minv11pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@minv11pt_20" />
            </td>
            <td class="colW1">{lbl:maxv11pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv11pt_1" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv11pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv11pt_20" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">
            </td>
            <td class="colW1"></td>
            <td class="colW2">
            </td>
          </tr>
          <!--<tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:swimming}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv12pt_21}</td>
            <td class="colW2">
              <xsl:value-of select="@minv12pt_21" />
            </td>
            <td class="colW1">{lbl:minv12pt_22}</td>
            <td class="colW2">
              <xsl:value-of select="@minv12pt_22" />
            </td>
            <td class="colW1">{lbl:maxv12pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv12pt_2" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv12pt_21}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv12pt_21" />
            </td>
            <td class="colW1">{lbl:maxv12pt_22}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv12pt_22" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
          </tr>-->
          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:othersports}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv99pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@minv99pt_1" />
            </td>
            <td class="colW1">{lbl:minv99pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@minv99pt_2" />
            </td>
            <td class="colW1">{lbl:minv99pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@minv99pt_3" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv99pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@minv99pt_20" />
            </td>
            <td class="colW1">{lbl:maxv99pt_1}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv99pt_1" />
            </td>
            <td class="colW1">{lbl:maxv99pt_2}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv99pt_2" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv99pt_3}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv99pt_3" />
            </td>
            <td class="colW1">{lbl:maxv99pt_20}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv99pt_20" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
           
          </tr>

          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:MixSportsParlay}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv99pt_9}</td>
            <td class="colW2">
              <xsl:value-of select="@minv99pt_9" />
            </td>
            <td class="colW1">{lbl:maxv99pt_9}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv99pt_9" />
            </td>
            <td class="colW1"></td>
            <td class="colW2"></td>
          </tr>

          <tr class="headerSportTypeDetail">
            <td colspan="6">{lbl:NumberGame}</td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv161pt_81}</td>
            <td class="colW2">
              <xsl:value-of select="@minv161pt_81" />
            </td>
            <td class="colW1">{lbl:minv161pt_83}</td>
            <td class="colW2">
              <xsl:value-of select="@minv161pt_83" />
            </td>
            <td class="colW1">{lbl:minv161pt_85}</td>
            <td class="colW2">
              <xsl:value-of select="@minv161pt_85" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv161pt_86}</td>
            <td class="colW2">
              <xsl:value-of select="@minv161pt_86" />
            </td>
            <td class="colW1">{lbl:minv161livept_85}</td>
            <td class="colW2">
              <xsl:value-of select="@minv161livept_85" />
            </td>
            <td class="colW1">{lbl:minv161livept_86}</td>
            <td class="colW2">
              <xsl:value-of select="@minv161livept_86" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:minv161livept_87}</td>
            <td class="colW2">
              <xsl:value-of select="@minv161livept_87" />
            </td>
            <td class="colW1">{lbl:maxv161pt_81}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv161pt_81" />
            </td>
            <td class="colW1">{lbl:maxv161pt_83}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv161pt_83" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv161pt_85}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv161pt_85" />
            </td>
            <td class="colW1">{lbl:maxv161pt_86}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv161pt_86" />
            </td>
            <td class="colW1">{lbl:maxv161livept_85}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv161livept_85" />
            </td>
          </tr>
          <tr>
            <td class="colW1">{lbl:maxv161livept_86}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv161livept_86" />
            </td>
            <td class="colW1">{lbl:maxv161livept_87}</td>
            <td class="colW2">
              <xsl:value-of select="@maxv161livept_87" />
            </td>
            <td class="colW1"></td>
            <td class="colW2"></td>
          </tr>
        </table>

      </xsl:when>
      <xsl:otherwise>
        <table class="tblDesc">
          <xsl:for-each select="info">
            <tr class="headerDetail" onclick="$('#info{{tbodylogid}}').slideToggle();" >
              <td colspan="6">{lbl:custInfo}</td>
            </tr>
            <tr>
              <td colspan="6" >
                <div style="display:block;" id="info{{tbodylogid}}">
                  <table class="tblDesc" >
                    <tr >
                      <td class="colW1">
                        {lbl:updMinPos}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@updminpos" />
                      </td>

                      <td class="colW1">
                        {lbl:updMaxPos}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@updmaxpos" />
                      </td>
                      <td class="colW1">
                        {lbl:checkPermission}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@checkpermission" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:language}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@language" />
                      </td>

                      <td class="colW1">
                        {lbl:subAccName}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@subaccname" />
                      </td>
                      <td class="colW1">
                        {lbl:writeLog}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@writelog" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:ip}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@ip" />
                      </td>

                      <td class="colW1">
                        {lbl:errCode}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@errcode" />
                      </td>
                      <td class="colW1">
                        {lbl:errMsg}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@errmsg" />
                      </td>

                    </tr>
                  </table>
                </div>
              </td>
            </tr>


          </xsl:for-each>
          <xsl:for-each select="minmaxpt" >
            <tr class="headerDetail" onclick="$('#minmaxpt{{tbodylogid}}').slideToggle();">
              <td colspan="6">{lbl:minMaxPT}</td>
            </tr>
            <tr>
              <td colspan="6">
                <div id="minmaxpt{{tbodylogid}}" style="display:none;">
                  <table class="tblDesc"  >
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">{lbl:soccer}</td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:minPT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minpt_1" />
                      </td>
                      <td class="colW1">
                        {lbl:minPT_2}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minpt_2" />
                      </td>
                      <td class="colW1">
                        {lbl:minPT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minpt_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:minPT_4}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minpt_4" />
                      </td>
                      <td class="colW1">
                        {lbl:minPT_5}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minpt_5" />
                      </td>
                      <td class="colW1">
                        {lbl:minPT_6}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minpt_6" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:minPT_7}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minpt_7" />
                      </td>
                      <td class="colW1">
                        {lbl:minPT_8}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minpt_8" />
                      </td>
                      <td class="colW1">
                        {lbl:minPT_9}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minpt_9" />
                      </td>
                    </tr>
                    <tr >

                      <td class="colW1">
                        {lbl:minPT_10}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minpt_10" />
                      </td>
                      <td class="colW1">
                        {lbl:minLivePT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minlivept_1" />
                      </td>
                      <td class="colW1">
                        {lbl:minLivePT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minlivept_3" />
                      </td>
                    </tr>
                    <tr >

                      <td class="colW1">
                        {lbl:minLivePT_7}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minlivept_7" />
                      </td>
                      <td class="colW1">
                        {lbl:minLivePT_8}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minlivept_8" />
                      </td>


                      <td class="colW1">
                        {lbl:maxPT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxpt_1" />
                      </td>
                    </tr>


                    <tr >
                      <td class="colW1">
                        {lbl:maxPT_2}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxpt_2" />
                      </td>
                      <td class="colW1">
                        {lbl:maxPT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxpt_3" />
                      </td>

                      <td class="colW1">
                        {lbl:maxPT_4}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxpt_4" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:maxPT_5}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxpt_5" />
                      </td>
                      <td class="colW1">
                        {lbl:maxPT_6}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxpt_6" />
                      </td>

                      <td class="colW1">
                        {lbl:maxPT_7}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxpt_7" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:maxPT_8}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxpt_8" />
                      </td>
                      <td class="colW1">
                        {lbl:maxPT_9}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxpt_9" />
                      </td>


                      <td class="colW1">
                        {lbl:maxPT_10}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxpt_10" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:maxLivePT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxlivept_1" />
                      </td>
                      <td class="colW1">
                        {lbl:maxLivePT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxlivept_3" />
                      </td>


                      <td class="colW1">
                        {lbl:maxLivePT_7}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxlivept_7" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:maxLivePT_8}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxlivept_8" />
                      </td>

                      <td class="colTitle"></td>
                      <td class="colValue">

                      </td>
                      <td class="colTitle"></td>
                      <td class="colValue">

                      </td>
                    </tr>


                    <tr class="headerSportTypeDetail">
                      <td colspan="6">{lbl:basketball}</td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:minV2PT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv2pt_1" />
                      </td>
                      <td class="colW1">
                        {lbl:minV2PT_2}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv2pt_2" />
                      </td>
                      <td class="colW1">
                        {lbl:minV2PT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv2pt_3" />
                      </td>

                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:minV2PT_7}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv2pt_7" />
                      </td>

                      <td class="colW1">
                        {lbl:minV2PT_8}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv2pt_8" />
                      </td>
                      <td class="colW1">
                        {lbl:minV2PT_9}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv2pt_9" />
                      </td>

                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:minV2PT_20}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv2pt_20" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV2PT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv2pt_1" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV2PT_2}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv2pt_2" />
                      </td>

                    </tr>
                    <tr>
                      <td class="colW1">
                        {lbl:maxV2PT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv2pt_3" />
                      </td>



                      <td class="colW1">
                        {lbl:maxV2PT_7}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv2pt_7" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV2PT_8}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv2pt_8" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:maxV2PT_20}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv2pt_20" />
                      </td>
                      <td class="colW1"></td>
                      <td class="colW2"></td>
                      <td class="colW1">

                      </td>
                      <td class="colW2">

                      </td>
                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">{lbl:football}</td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:minV3PT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv3pt_1" />
                      </td>
                      <td class="colW1">
                        {lbl:minV3PT_2}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv3pt_2" />
                      </td>


                      <td class="colW1">
                        {lbl:minV3PT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv3pt_3" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:minV3PT_20}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv3pt_20" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV3PT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv3pt_1" />
                      </td>


                      <td class="colW1">
                        {lbl:maxV3PT_2}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv3pt_2" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:maxV3PT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv3pt_3" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV3PT_20}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv3pt_20" />
                      </td>
                      <td class="colW1">

                      </td>
                      <td class="colW2">

                      </td>
                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">{lbl:Tennis}</td>
                    </tr>
                    <tr >

                      <td class="colW1">
                        {lbl:minV5PT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv5pt_1" />
                      </td>
                      <td class="colW1">
                        {lbl:minV5PT_2}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv5pt_2" />
                      </td>
                      <td class="colW1">
                        {lbl:minV5PT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv5pt_3" />
                      </td>
                    </tr>
                    <tr >

                      <td class="colW1">
                        {lbl:minV5PT_20}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv5pt_20" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV5PT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv5pt_1" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV5PT_2}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv5pt_2" />
                      </td>
                    </tr>
                    <tr >

                      <td class="colW1">
                        {lbl:maxV5PT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv5pt_3" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV5PT_20}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv5pt_20" />
                      </td>
                      <td class="colW1">

                      </td>
                      <td class="colW2">

                      </td>
                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">{lbl:baseball}</td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:minV8PT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv8pt_1" />
                      </td>
                      <td class="colW1">
                        {lbl:minV8PT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv8pt_3" />
                      </td>
                      <td class="colW1">
                        {lbl:minV8PT_20}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv8pt_20" />
                      </td>

                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:maxV8PT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv8pt_1" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV8PT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv8pt_3" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV8PT_20}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv8pt_20" />
                      </td>

                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">{lbl:golf}</td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:minV10PT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv10pt_1" />
                      </td>
                      <td class="colW1">
                        {lbl:minV10PT_2}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv10pt_2" />
                      </td>
                      <td class="colW1">
                        {lbl:minV10PT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv10pt_3" />
                      </td>

                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:minV10PT_20}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv10pt_20" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV10PT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv10pt_1" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV10PT_2}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv10pt_2" />
                      </td>

                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:maxV10PT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv10pt_3" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV10PT_20}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv10pt_20" />
                      </td>
                      <td class="colW1">

                      </td>
                      <td class="colW2">

                      </td>
                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">{lbl:Motosports}</td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:minV11PT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv11pt_1" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV11PT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv11pt_1" />
                      </td>
                      <td class="colW1"></td>
                      <td class="colW2" >

                      </td>

                    </tr>
                    <tr>
                      <td class="colW1">
                        {lbl:minV11PT_20}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv11pt_20" />
                      </td>

                      <td class="colW1">
                        {lbl:maxV11PT_20}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv11pt_20" />
                      </td>
                      <td class="colW1">

                      </td>
                      <td class="colW2">

                      </td>
                    </tr>
                    <tr class="headerSportTypeDetail">
                      <td colspan="6">{lbl:othersports}</td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:minV99PT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv99pt_1" />
                      </td>
                      <td class="colW1">
                        {lbl:minV99PT_2}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv99pt_2" />
                      </td>
                      <td class="colW1">
                        {lbl:minV99PT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv99pt_3" />
                      </td>
                    </tr>
                    <tr >

                      <td class="colW1">
                        {lbl:minV99PT_20}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@minv99pt_20" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV99PT_1}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv99pt_1" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV99PT_2}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv99pt_2" />
                      </td>
                    </tr>
                    <tr >
                      <td class="colW1">
                        {lbl:maxV99PT_3}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv99pt_3" />
                      </td>
                      <td class="colW1">
                        {lbl:maxV99PT_20}
                      </td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv99pt_20" />
                      </td>
                      <td class="colW1">

                      </td>
                      <td class="colW2">

                      </td>
                    </tr>

                    <tr class="headerSportTypeDetail">
                      <td colspan="6">{lbl:MixSportsParlay}</td>
                    </tr>
                    <tr>
                      <td class="colW1">{lbl:minv99pt_9}</td>
                      <td class="colW2">
                        <xsl:value-of select="@minv99pt_9" />
                      </td>
                      <td class="colW1">{lbl:maxv99pt_9}</td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv99pt_9" />
                      </td>
                      <td class="colW1"></td>
                      <td class="colW2"></td>
                    </tr>

                    <tr class="headerSportTypeDetail">
                      <td colspan="6">{lbl:NumberGame}</td>
                    </tr>
                    <tr>
                      <td class="colW1">{lbl:minv161pt_81}</td>
                      <td class="colW2">
                        <xsl:value-of select="@minv161pt_81" />
                      </td>
                      <td class="colW1">{lbl:minv161pt_83}</td>
                      <td class="colW2">
                        <xsl:value-of select="@minv161pt_83" />
                      </td>
                      <td class="colW1">{lbl:minv161pt_85}</td>
                      <td class="colW2">
                        <xsl:value-of select="@minv161pt_85" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">{lbl:minv161pt_86}</td>
                      <td class="colW2">
                        <xsl:value-of select="@minv161pt_86" />
                      </td>
                      <td class="colW1">{lbl:minv161livept_85}</td>
                      <td class="colW2">
                        <xsl:value-of select="@minv161livept_85" />
                      </td>
                      <td class="colW1">{lbl:minv161livept_86}</td>
                      <td class="colW2">
                        <xsl:value-of select="@minv161livept_86" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">{lbl:minv161livept_87}</td>
                      <td class="colW2">
                        <xsl:value-of select="@minv161livept_87" />
                      </td>
                      <td class="colW1">{lbl:maxv161pt_81}</td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv161pt_81" />
                      </td>
                      <td class="colW1">{lbl:maxv161pt_83}</td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv161pt_83" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">{lbl:maxv161pt_85}</td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv161pt_85" />
                      </td>
                      <td class="colW1">{lbl:maxv161pt_86}</td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv161pt_86" />
                      </td>
                      <td class="colW1">{lbl:maxv161livept_85}</td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv161livept_85" />
                      </td>
                    </tr>
                    <tr>
                      <td class="colW1">{lbl:maxv161livept_86}</td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv161livept_86" />
                      </td>
                      <td class="colW1">{lbl:maxv161livept_87}</td>
                      <td class="colW2">
                        <xsl:value-of select="@maxv161livept_87" />
                      </td>
                      <td class="colW1"></td>
                      <td class="colW2"></td>
                    </tr>
                  </table>
                </div>
              </td>
            </tr>

          </xsl:for-each>
        </table>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
