<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="age_custsubins">
    <table class="tblDesc">
      <tr>





        <td class="colW1">{lbl:username}</td>
        <td class="colW2">
          <xsl:value-of select="@username" />
        </td>
        <td class="colW1">{lbl:fName}</td>
        <td class="colW2">
          <xsl:value-of select="@fname" />
        </td>
        <td class="colW1">{lbl:lName}</td>
        <td class="colW2">
          <xsl:value-of select="@lname" />
        </td>
      </tr>
      <tr>



        <td class="colW1">{lbl:success}</td>
        <td class="colW2">
          <xsl:value-of select="@success" />
        </td>

        <td class="colW1">{lbl:language}</td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>

        <td class="colW1">{lbl:pageAccess}</td>
        <td class="colW2">
         <xsl:value-of select="@pageaccess" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:loginIP}</td>
        <td class="colW2">
          <xsl:value-of select="@loginip" />
        </td>
        <!--<td class="colW1">{lbl:userID}</td>
        <td class="colW2">
          <xsl:value-of select="@userid" />
        </td>-->
        <!--<xsl:choose>
                    <xsl:when test="$IsInternal = 0">
                        <td class="colW1"></td>
                        <td class="colW2">

                        </td>
                    </xsl:when>
                    <xsl:otherwise>
                        <td class="colW1">{lbl:userID}</td>
                        <td class="colW2">

                            <xsl:value-of select="@userid" />
                        </td>

                    </xsl:otherwise>
                </xsl:choose>-->

        <td class="colW1">{lbl:errCode}</td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>

        <td class="colW1">{lbl:errMsg}</td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
      </tr>

      <xsl:choose>
        <xsl:when test="$IsInternal = 0">



        </xsl:when>
        <xsl:otherwise>
          <tr>
            <td class="colW1">{lbl:subAccID}</td>
            <td class="colW2">

              <xsl:value-of select="@subaccid" />
            </td>
            <td class="colW1">{lbl:loginSubAccID}</td>
            <td class="colW2">

              <xsl:value-of select="@loginsubaccid" />
            </td>
            <td class="colW1"></td>
            <td class="colW2">

            </td>
          </tr>
        </xsl:otherwise>
      </xsl:choose>



    </table>







  </xsl:template>
</xsl:stylesheet>
